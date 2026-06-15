using InlämningWebb1.Application.Features.Auth.Commands.Login;
using InlämningWebb1.Application.Features.Auth.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InlämningWebb1.API.Controllers;

/// <summary>
/// Handles authentication requests.
/// This is the only controller that does NOT require [Authorize] —
/// clients must be able to call it to obtain a token in the first place.
/// </summary>
[ApiController]
[Route("api/[controller]")]  // → api/auth
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticate with username and password. Returns a JWT Bearer token on success.
    /// Use the returned token in the Authorization header for all protected endpoints:
    ///   Authorization: Bearer {token}
    /// </summary>
    /// <param name="dto">Login credentials from the request body.</param>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(dto.Username, dto.Password);
        var result = await _mediator.Send(command, cancellationToken);

        // Handler returns null when credentials are invalid → 401 Unauthorized
        return result is null ? Unauthorized() : Ok(result);
    }
}
