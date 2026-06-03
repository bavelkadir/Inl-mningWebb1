using InlämningWebb1.Domain.Entities;
using InlämningWebb1.Domain.Interfaces;
using InlämningWebb1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InlämningWebb1.Infrastructure.Repositories;

/// <summary>Category repository. Inherits generic CRUD and adds category-specific queries.</summary>
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => await _context.Categories
            .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);
}
