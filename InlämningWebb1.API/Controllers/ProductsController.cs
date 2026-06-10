using InlämningWebb1.Application.Features.Products.Commands.CreateProduct;
using InlämningWebb1.Application.Features.Products.Commands.DeleteProduct;
using InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;
using InlämningWebb1.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InlämningWebb1.API.Controllers;

/// <summary>
/// Handles HTTP requests for the Product resource.
/// This controller is intentionally thin:
///   - It receives DTOs from the HTTP body.
///   - It builds commands/queries and sends them through MediatR.
///   - It maps handler results to HTTP responses.
/// No business logic, no AutoMapper, no repositories here.
/// </summary>
[ApiController]
[Route("api/[controller]")]  // → api/products
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all products.</summary>
    /// <returns>List of all products as ProductDto objects.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    /// <summary>Get a single product by ID.</summary>
    /// <param name="id">The product's unique identifier (GUID).</param>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Create a new product.</summary>
    /// <param name="dto">Name, Price, and CategoryId from the JSON request body.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        // Build the command from the DTO manually — no AutoMapper needed here.
        // The controller stays thin and has no extra dependencies.
        var command = new CreateProductCommand(dto.Name, dto.Price, dto.CategoryId);
        var result = await _mediator.Send(command, cancellationToken);

        // 201 Created with a Location header pointing to GET /api/products/{id}
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing product (full replacement — PUT semantics).</summary>
    /// <param name="id">Product ID from the URL route. Takes priority — clients do not send the ID in the body.</param>
    /// <param name="dto">New Name, Price, and CategoryId from the JSON body.</param>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductDto dto,
        CancellationToken cancellationToken)
    {
        // Combine the route id with the body DTO to build a complete command
        var command = new UpdateProductCommand(id, dto.Name, dto.Price, dto.CategoryId);
        var success = await _mediator.Send(command, cancellationToken);
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
