using InlämningWebb1.Domain.Entities;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Query to retrieve a single product by its unique ID.
/// Returns null if no product with the given ID exists — the controller then returns 404.
/// </summary>
/// <param name="Id">The unique identifier of the product to retrieve.</param>
public record GetProductByIdQuery(Guid Id) : IRequest<Product?>;
