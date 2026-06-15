using InlämningWebb1.Application.Common.Interfaces;
using InlämningWebb1.Domain.Interfaces;
using InlämningWebb1.Infrastructure.Persistence;
using InlämningWebb1.Infrastructure.Repositories;
using InlämningWebb1.Infrastructure.Services;
using InlämningWebb1.Infrastructure.Settings;
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
        // Database context — reads connection string from appsettings.json
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        // JWT settings — reads each key from configuration using the standard indexer.
        // In development: IConfiguration merges appsettings.json and User Secrets automatically,
        // so Jwt:Key comes from User Secrets while Issuer/Audience come from appsettings.json.
        services.Configure<JwtSettings>(options =>
        {
            var s = configuration.GetSection(JwtSettings.SectionName);
            options.Key              = s["Key"]              ?? string.Empty;
            options.Issuer           = s["Issuer"]           ?? string.Empty;
            options.Audience         = s["Audience"]         ?? string.Empty;
            options.ExpiresInMinutes = int.TryParse(s["ExpiresInMinutes"], out var m) ? m : 60;
        });

        // Token service — generates signed JWT tokens (stateless → Singleton is safe)
        services.AddSingleton<ITokenService, TokenService>();

        // User service — validates credentials against the in-memory user list (Singleton is safe)
        services.AddSingleton<IUserService, UserService>();

        return services;
    }
}
