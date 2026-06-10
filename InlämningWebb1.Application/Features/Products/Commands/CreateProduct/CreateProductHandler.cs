using AutoMapper;
using InlämningWebb1.Application.Features.Products.DTOs;
using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handler for CreateProductCommand.
/// Builds the domain entity, persists it, then maps it to a ProductDto for the response.
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>Creates the product entity, saves it, and returns the DTO.</summary>
    public async Task<ProductDto> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        // Build the domain entity. The ID is generated here — the client never decides this.
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        await _productRepository.AddAsync(product, cancellationToken);

        // Map the saved entity to a DTO — the controller only sees ProductDto
        return _mapper.Map<ProductDto>(product);
    }
}
