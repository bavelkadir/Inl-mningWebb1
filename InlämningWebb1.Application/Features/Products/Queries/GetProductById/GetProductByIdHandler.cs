using AutoMapper;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Queries.GetProductById;

/// <summary>
/// Handler for GetProductByIdQuery.
/// Returns a mapped ProductDto, or null when the product does not exist.
/// </summary>
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductByIdHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>Fetches the product and maps it to a DTO. Returns null if not found.</summary>
    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        // _mapper.Map respects null — if product is null, it returns null
        return product is null ? null : _mapper.Map<ProductDto>(product);
    }
}
