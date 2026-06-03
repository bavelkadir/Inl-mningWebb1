using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using InlämningWebb1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InlämningWebb1.Infrastructure.Repositories;

/// <summary>Product repository. Inherits generic CRUD and adds product-specific queries.</summary>
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Product>> GetByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken);
}
