namespace InlämningWebb1.Application.Features.Products.DTOs;

/// <summary>
/// The shape of a product as seen by API clients.
/// Contains only what the client needs — no navigation properties,
/// no EF Core metadata, and no internal domain concerns.
/// </summary>
public record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    Guid CategoryId);
