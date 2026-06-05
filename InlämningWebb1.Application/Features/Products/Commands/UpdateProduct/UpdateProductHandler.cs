using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Handler for UpdateProductCommand.
/// Loads the existing entity, applies new values, and saves.
/// Returns false (→ 404) if the product does not exist.
/// </summary>
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>Applies updated values to the entity and persists the changes.</summary>
    public async Task<bool> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        // First check that the product actually exists — return false if not
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return false;

        // Overwrite all fields (PUT = full replacement, not partial)
        product.Name = request.Name;
        product.Price = request.Price;
        product.CategoryId = request.CategoryId;

        // Repository calls _context.SaveChangesAsync() internally
        await _productRepository.UpdateAsync(product);

        return true;
    }
}
