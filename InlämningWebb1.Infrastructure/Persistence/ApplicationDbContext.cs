using InlämningWebb1.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InlämningWebb1.Infrastructure.Persistence;

/// <summary>The EF Core database context — the main entry point to the database.</summary>
public class ApplicationDbContext : DbContext
{
    // DbContextOptions contains the connection string and provider (SQL Server).
    // It is passed in from DependencyInjection.cs via AddDbContext().
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // DbSet represents a table in the database.
    // EF Core uses these to generate SQL queries (SELECT, INSERT, UPDATE, DELETE).
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Instead of configuring tables directly here, we use separate configuration classes
        // (CategoryConfiguration, ProductConfiguration). This method scans the assembly
        // and automatically applies all of them.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
