namespace InlämningWebb1.Application.Features.Products.DTOs;

/// <summary>
/// Data the client sends in the PUT /api/products/{id} request body.
/// Id comes from the URL route parameter, not from this DTO.
/// </summary>
public record UpdateProductDto(
    string Name,
    decimal Price,
    Guid CategoryId);
