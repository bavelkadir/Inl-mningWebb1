using InlämningWebb1.Domain.Entities;

namespace InlämningWebb1.Domain.Interfaces;

/// <summary>Category-specific repository. Extends the generic CRUD contract.</summary>
public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
