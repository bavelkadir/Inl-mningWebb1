using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Command to update an existing product (full replacement — PUT semantics).
/// Returns true if the update succeeded, false if the product was not found.
/// </summary>
/// <param name="Id">ID of the product to update (comes from the URL route in the controller).</param>
/// <param name="Name">New name for the product.</param>
/// <param name="Price">New price for the product.</param>
/// <param name="CategoryId">New category for the product.</param>
public record UpdateProductCommand(
    Guid Id,
    string Name,
    decimal Price,
    Guid CategoryId) : IRequest<bool>;
