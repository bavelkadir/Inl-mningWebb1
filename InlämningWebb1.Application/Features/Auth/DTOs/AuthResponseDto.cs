namespace InlämningWebb1.Application.Features.Auth.DTOs;

/// <summary>
/// The response returned after a successful login.
/// The client stores this token and sends it as "Authorization: Bearer {Token}" on every request.
/// </summary>
public record AuthResponseDto(
    string Token,
    string Username,
    string Role);
