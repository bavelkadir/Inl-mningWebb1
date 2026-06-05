using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;

/// <summary>
/// Handler for GetAllProductsQuery.
/// MediatR finds this class automatically because it implements
/// IRequestHandler&lt;GetAllProductsQuery, IEnumerable&lt;Product&gt;&gt;.
/// </summary>
public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _productRepository;

    /// <summary>IProductRepository is injected by the DI container automatically.</summary>
    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Executes the query. Called by MediatR when a GetAllProductsQuery is sent.
    /// </summary>
    /// <param name="request">The query object (no data needed here).</param>
    /// <param name="cancellationToken">Lets the caller cancel a long-running DB call.</param>
    public async Task<IEnumerable<Product>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync(cancellationToken);
    }
}
