using AutoMapper;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetAllProducts;

/// <summary>
/// Handler for GetAllProductsQuery.
/// Fetches domain entities from the repository, then maps them to DTOs
/// before returning — the API layer sees only ProductDto objects.
/// </summary>
public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetAllProductsHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>Loads all products and maps them to the DTO shape.</summary>
    public async Task<IEnumerable<ProductDto>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        // AutoMapper uses the CreateMap<Product, ProductDto>() rule from ProductMappingProfile
        // to transform every item in the collection automatically.
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}
