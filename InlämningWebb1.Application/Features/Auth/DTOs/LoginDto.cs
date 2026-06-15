namespace InlämningWebb1.Application.Features.Auth.DTOs;

/// <summary>
/// Data the client sends in the POST /api/auth/login request body.
/// </summary>
public record LoginDto(string Username, string Password);
