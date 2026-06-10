using InlämningWebb1.Application.Features.Products.DTOs;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Command to create a new product.
/// Now returns ProductDto instead of the raw Product entity — the API layer
/// receives a DTO directly without knowing about the domain entity.
/// </summary>
/// <param name="Name">The display name of the product.</param>
/// <param name="Price">The price (always decimal for money, never float).</param>
/// <param name="CategoryId">The category this product belongs to.</param>
public record CreateProductCommand(
    string Name,
    decimal Price,
    Guid CategoryId) : IRequest<ProductDto>;
