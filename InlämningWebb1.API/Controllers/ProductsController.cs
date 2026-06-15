using InlämningWebb1.Application.Features.Products.Commands.CreateProduct;
using InlämningWebb1.Application.Features.Products.Commands.DeleteProduct;
using InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;
using InlämningWebb1.Application.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InlämningWebb1.API.Controllers;

/// <summary>
/// Handles HTTP requests for the Product resource.
///
/// Authorization rules:
///   [Authorize] at class level  → every endpoint requires a valid JWT token.
///   [Authorize(Roles = "Admin")] on write endpoints → only Admin tokens are accepted.
///
/// If no token is sent → 401 Unauthorized.
/// If a User token is sent to an Admin endpoint → 403 Forbidden.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]  // All endpoints in this controller require authentication
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Get all products. Requires: any authenticated user (User or Admin).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);
        return Ok(products);
    }

    /// <summary>Get a single product by ID. Requires: any authenticated user.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Create a new product. Requires: Admin role.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]  // Only tokens with role=Admin reach the handler
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductDto dto,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(dto.Name, dto.Price, dto.CategoryId);
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing product. Requires: Admin role.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductDto dto,
        CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, dto.Name, dto.Price, dto.CategoryId);
        var success = await _mediator.Send(command, cancellationToken);
        return success ? NoContent() : NotFound();
    }

    /// <summary>Delete a product. Requires: Admin role.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var success = await _mediator.Send(new DeleteProductCommand(id), cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
