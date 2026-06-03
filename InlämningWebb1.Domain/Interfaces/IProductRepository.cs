using InlämningWebb1.Domain.Entities;

namespace InlämningWebb1.Domain.Interfaces;

/// <summary>Product-specific repository. Extends the generic CRUD contract.</summary>
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);
}
