using InlämningWebb1.Application.Features.Products.Commands.CreateProduct;
using InlämningWebb1.Application.Features.Products.Commands.DeleteProduct;
using InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;
using InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;
using InlämningWebb1.Application.Features.Products.Queries.GetProductById;
using InlämningWebb1.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InlämningWebb1.API.Controllers;

/// <summary>
/// Handles all HTTP requests for the Product resource.
/// Thin by design — no business logic here.
/// The controller only packages requests and delegates to MediatR.
/// </summary>
[ApiController]
[Route("api/[controller]")]  // Route: api/products
public class ProductsController : ControllerBase
{
    // IMediator is the only dependency — this controller knows nothing about repositories or EF Core
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all products.</summary>
    /// <returns>A list of all products in the database.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    /// <summary>Get a single product by its ID.</summary>
    /// <param name="id">The product's unique identifier (GUID).</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        // Handler returns null when nothing was found — we translate that to 404
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Create a new product.</summary>
    /// <param name="command">Name, Price, and CategoryId from the JSON request body.</param>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(command, cancellationToken);

        // 201 Created — includes a Location header pointing to GET /api/products/{id}
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    /// <summary>Update an existing product (full replacement).</summary>
    /// <param name="id">Product ID from the URL — takes priority over any ID in the body.</param>
    /// <param name="command">Updated Name, Price, and CategoryId from the JSON body.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductCommand command,
        CancellationToken cancellationToken)
    {
        // The record "with" expression creates a copy of the command with the Id overridden
        // by the route's id. This means the client only sends Name/Price/CategoryId in the body.
        var commandWithId = command with { Id = id };

        var success = await _mediator.Send(commandWithId, cancellationToken);

        return success ? NoContent() : NotFound();
    }

    /// <summary>Delete a product.</summary>
    /// <param name="id">The ID of the product to delete.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);

        return success ? NoContent() : NotFound();
    }
}
