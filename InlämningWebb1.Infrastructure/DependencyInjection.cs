using InlämningWebb1.Domain.Interfaces;
using InlämningWebb1.Infrastructure.Persistence;
using InlämningWebb1.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InlämningWebb1.Infrastructure;

/// <summary>Extension method that registers all Infrastructure services into the DI container.</summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register the database context. EF Core reads the connection string from appsettings.json
        // and uses SQL Server as the database provider.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Register repositories. When something asks for IProductRepository,
        // ASP.NET Core will create and inject a ProductRepository automatically.
        // Scoped = one instance created per HTTP request, then disposed.
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
