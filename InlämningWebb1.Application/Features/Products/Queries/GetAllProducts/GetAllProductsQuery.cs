using InlämningWebb1.Application.Features.Products.DTOs;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;

/// <summary>
/// Query to retrieve all products.
/// Now returns IEnumerable&lt;ProductDto&gt; — the API layer never sees a raw domain entity.
/// </summary>
public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
