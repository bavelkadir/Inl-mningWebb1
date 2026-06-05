using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InlämningWebb1.Application;

/// <summary>
/// Extension method that registers all Application-layer services (MediatR handlers)
/// into the ASP.NET Core DI container. Called once from Program.cs.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds MediatR and auto-discovers every IRequestHandler in this assembly.
    /// You never have to register handlers one by one — MediatR scans and wires them up.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // RegisterServicesFromAssembly scans the Application assembly at startup and registers
        // all classes that implement IRequestHandler<TRequest, TResponse> automatically.
        // When the controller sends a command/query, MediatR looks up the correct handler here.
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
