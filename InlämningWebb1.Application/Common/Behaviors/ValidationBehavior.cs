using FluentValidation;
using MediatR;

namespace InlämningWebb1.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that runs all registered FluentValidation validators
/// for a request BEFORE the handler executes.
///
/// How it works:
///   1. MediatR calls Handle() with the request.
///   2. We look up all IValidator&lt;TRequest&gt; registered in DI.
///   3. If any rules fail, we throw ValidationException — the handler never runs.
///   4. If all rules pass, we call next() which continues to the handler.
///
/// Registered as an open generic in DI, so it automatically applies to every
/// command and query — no per-handler code needed.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // DI injects ALL validators registered for this TRequest type.
    // If none exist, the collection is empty (no null check needed).
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Intercepts the request, runs validation, and either throws or continues.
    /// </summary>
    /// <param name="request">The incoming command or query.</param>
    /// <param name="next">Delegate that calls the next behavior or the handler itself.</param>
    /// <param name="cancellationToken">Cancellation support for async validators.</param>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // No validators registered for this request type → skip validation entirely
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        // Run all validators concurrently — supports async validators (e.g., DB uniqueness checks)
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Collect every failure from every validator into a flat list
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToList();

        if (failures.Count != 0)
            // Throw all failures at once — the global exception handler in Program.cs
            // catches this and returns a structured 400 Bad Request response.
            throw new ValidationException(failures);

        // Validation passed → continue to the actual handler
        return await next();
    }
}
