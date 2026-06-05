using InlämningWebb1.Application;
using InlämningWebb1.Infrastructure;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers — enables [ApiController] classes and attribute routing
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
        // Domain entities have circular navigation (Product → Category → Products).
        // IgnoreCycles stops the JSON serializer from crashing on that loop.
        // This is a temporary measure — DTOs in the next step will remove the need for this.
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// OpenAPI spec generation — produces /openapi/v1.json (used by Scalar UI below)
builder.Services.AddOpenApi();

// Application layer: registers MediatR + all handlers from the Application assembly
builder.Services.AddApplication();

// Infrastructure layer: EF Core DbContext + repositories
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // Serves the raw OpenAPI spec at /openapi/v1.json
    app.MapOpenApi();

    // Scalar: modern interactive API UI — navigate to /scalar/v1 in the browser
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Maps all routes declared via [Route] / [HttpGet] etc. on controller classes
app.MapControllers();

app.Run();
