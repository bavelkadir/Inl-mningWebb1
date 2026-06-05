using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Command to delete a product by its ID.
/// Returns true if the product was found and deleted, false if it did not exist.
/// </summary>
/// <param name="Id">The unique identifier of the product to delete.</param>
public record DeleteProductCommand(Guid Id) : IRequest<bool>;
