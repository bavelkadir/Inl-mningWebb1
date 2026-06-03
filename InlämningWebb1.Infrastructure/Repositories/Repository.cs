using InlämningWebb1.Domain.Interfaces;
using InlämningWebb1.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InlämningWebb1.Infrastructure.Repositories;

/// <summary>
/// Generic repository base class. Contains the shared CRUD logic that
/// ProductRepository and CategoryRepository both inherit.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    // _context gives us access to the database (via EF Core).
    // _dbSet is the specific table for type T (e.g. DbSet<Product> or DbSet<Category>).
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>(); // Resolves to the correct DbSet based on T
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet.FindAsync([id], cancellationToken); // Looks up by primary key

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbSet.ToListAsync(cancellationToken); // SELECT * FROM table

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken); // Marks the entity as "to be inserted"
        await _context.SaveChangesAsync(cancellationToken); // Sends the INSERT to the database
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity); // Marks the entity as "modified"
        await _context.SaveChangesAsync(); // Sends the UPDATE to the database
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _dbSet.Remove(entity); // Marks the entity as "to be deleted"
            await _context.SaveChangesAsync(cancellationToken); // Sends the DELETE to the database
        }
    }
}
