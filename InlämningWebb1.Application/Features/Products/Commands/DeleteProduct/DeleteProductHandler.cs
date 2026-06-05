using InlämningWebb1.Domain.Interfaces;
using MediatR;

namespace InlämningWebb1.Application.Features.Products.Commands.DeleteProduct;

/// <summary>
/// Handler for DeleteProductCommand.
/// Verifies the product exists before deleting, returns false (→ 404) if not found.
/// </summary>
public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>Deletes the product with the given ID. Returns false if it does not exist.</summary>
    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        // Confirm the product exists — DeleteAsync will silently no-op on a missing ID
        // so we check first to give a meaningful 404 instead of a silent 204
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null) return false;

        await _productRepository.DeleteAsync(request.Id, cancellationToken);

        return true;
    }
}
