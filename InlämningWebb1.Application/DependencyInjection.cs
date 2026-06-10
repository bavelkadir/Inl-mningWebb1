using FluentValidation;
using InlämningWebb1.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InlämningWebb1.Application;

/// <summary>
/// Extension method that registers all Application-layer services into the DI container.
/// Called once from Program.cs via builder.Services.AddApplication().
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR: discovers and registers all IRequestHandler implementations in this assembly.
        // No manual handler registration needed — scan does it automatically.
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        // ValidationBehavior: runs BEFORE every handler for every request type.
        // Registered as open generic — one registration covers ALL commands and queries.
        // If a request has no validator, the behavior skips validation silently.
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // AutoMapper: discovers and registers all Profile subclasses in this assembly.
        // AddMaps() scans for Profile subclasses — the AutoMapper 16 way of doing assembly scanning.
        // Enables IMapper injection in handlers.
        services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

        // FluentValidation: discovers and registers all AbstractValidator<T> classes.
        // Enables IEnumerable<IValidator<TRequest>> injection in ValidationBehavior.
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
