using InlämningWebb1.Domain.Entities;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;

/// <summary>
/// Query to retrieve all products from the database.
/// Implements IRequest so MediatR knows this is a request that returns IEnumerable of Product.
/// No input parameters needed — we want every product.
/// </summary>
public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;
