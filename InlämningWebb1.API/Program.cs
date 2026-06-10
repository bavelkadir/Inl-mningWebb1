using FluentValidation;
using InlämningWebb1.Application;
using InlämningWebb1.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers — enables [ApiController] classes and attribute routing.
// ReferenceHandler.IgnoreCycles is no longer needed now that handlers return DTOs,
// which have no circular navigation properties.
builder.Services.AddControllers();

// OpenAPI spec generation — produces /openapi/v1.json (consumed by Scalar UI below)
builder.Services.AddOpenApi();

// Application layer: MediatR handlers + ValidationBehavior + AutoMapper + FluentValidation validators
builder.Services.AddApplication();

// Infrastructure layer: EF Core DbContext + repositories
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Global exception handler — must be first so it catches exceptions from all middleware after it.
// Translates FluentValidation.ValidationException → 400 Bad Request with structured error list.
// All other unhandled exceptions → 500 Internal Server Error.
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (feature?.Error is ValidationException validationException)
        {
            // Return all validation failures as a structured JSON list
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = validationException.Errors
                .Select(e => new { property = e.PropertyName, message = e.ErrorMessage });

            await context.Response.WriteAsJsonAsync(new { errors });
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    });
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();              // Raw spec at /openapi/v1.json
    app.MapScalarApiReference();   // Interactive UI at /scalar/v1
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
