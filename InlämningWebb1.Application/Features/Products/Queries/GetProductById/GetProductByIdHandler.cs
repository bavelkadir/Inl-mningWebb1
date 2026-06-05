using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Handler for GetProductByIdQuery.
/// Returns null when the product does not exist — the controller translates null → 404 NotFound.
/// </summary>
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>Fetches a single product by ID. Returns null if not found.</summary>
    public async Task<Product?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}
