namespace InlämningWebb1.Application.Features.Products.DTOs;

/// <summary>
/// Data the client sends in the POST /api/products request body.
/// Id is intentionally absent — the server generates it; clients never choose IDs.
/// </summary>
public record CreateProductDto(
    string Name,
    decimal Price,
    Guid CategoryId);
