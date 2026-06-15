using InlämningWebb1.Application.Features.Auth.DTOs;
using MediatR;

namespace InlämningWebb1.Application.Features.Auth.Commands.Login;

/// <summary>
/// Command to authenticate a user with username and password.
/// Returns AuthResponseDto (with the JWT token) on success, or null on failure.
/// The controller translates null → 401 Unauthorized.
/// </summary>
/// <param name="Username">The user's login name.</param>
/// <param name="Password">The user's plaintext password.</param>
public record LoginCommand(string Username, string Password) : IRequest<AuthResponseDto?>;
