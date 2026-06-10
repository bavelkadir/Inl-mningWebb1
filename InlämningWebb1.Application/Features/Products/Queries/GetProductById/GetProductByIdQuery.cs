using InlämningWebb1.Application.Features.Products.DTOs;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Query to retrieve a single product by ID.
/// Returns null if not found — the controller translates null to 404 NotFound.
/// </summary>
/// <param name="Id">The unique identifier of the product to retrieve.</param>
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
