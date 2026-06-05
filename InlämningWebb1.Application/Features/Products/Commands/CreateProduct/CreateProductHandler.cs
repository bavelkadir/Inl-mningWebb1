using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Handler for CreateProductCommand.
/// Builds the Product entity from command data, persists it, and returns it to the caller.
/// </summary>
public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>Creates a new Product, saves it to the database, and returns the saved entity.</summary>
    public async Task<Product> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        // Build the domain entity from the incoming command data.
        // The ID is generated here — the client never decides the ID.
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        // Persist via the repository — SaveChangesAsync runs inside AddAsync
        await _productRepository.AddAsync(product, cancellationToken);

        // Return the saved entity so the controller can include it in the 201 Created response
        return product;
    }
}
