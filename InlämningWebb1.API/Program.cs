using System.Text;
using FluentValidation;
using InlämningWebb1.Application;
using InlämningWebb1.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ─── Controllers ────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─── OpenAPI / Scalar ───────────────────────────────────────────────────────
builder.Services.AddOpenApi();

// ─── JWT Authentication ──────────────────────────────────────────────────────
// This middleware validates the Bearer token on every incoming request.
// It reads the signing key from configuration — in development this merges
// appsettings.json (Issuer, Audience) with User Secrets (Key).
builder.Services.AddAuthentication(options =>
{
    // Set JWT Bearer as the default scheme for both authentication and challenge
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer           = true,
        ValidateAudience         = true,
        ValidateLifetime         = true,   // rejects expired tokens
        ValidateIssuerSigningKey = true,   // verifies the signature with our secret key

        ValidIssuer   = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        // The signing key — loaded from User Secrets in dev, environment variable in prod.
        // Never read from appsettings.json directly.
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

// Required alongside AddAuthentication — enables [Authorize] attribute evaluation
builder.Services.AddAuthorization();

// ─── Application + Infrastructure ───────────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ═══════════════════════════════════════════════════════════════════════════
var app = builder.Build();
// ═══════════════════════════════════════════════════════════════════════════

// Global exception handler — catches ValidationException → 400, everything else → 500.
// Must be first in the pipeline so it wraps all subsequent middleware.
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();

        if (feature?.Error is ValidationException validationException)
        {
            context.Response.StatusCode  = StatusCodes.Status400BadRequest;
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
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// ─── Authentication & Authorization middleware ───────────────────────────────
// ORDER MATTERS: Authentication must come before Authorization.
// UseAuthentication reads the token and populates HttpContext.User.
// UseAuthorization then checks the [Authorize] attributes against HttpContext.User.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
