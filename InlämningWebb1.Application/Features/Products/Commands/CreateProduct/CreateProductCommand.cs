using InlämningWebb1.Domain.Entities;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Command to create a new product.
/// Carries all the data needed to build the Product entity.
/// Returns the created Product so the controller can send it back in the 201 response.
/// </summary>
/// <param name="Name">The display name of the product.</param>
/// <param name="Price">The price (always use decimal for money, never float).</param>
/// <param name="CategoryId">The category this product belongs to.</param>
public record CreateProductCommand(
    string Name,
    decimal Price,
    Guid CategoryId) : IRequest<Product>;
