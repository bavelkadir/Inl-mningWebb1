using InlämningWebb1.Application.Common.Interfaces;
using InlämningWebb1.Application.Features.Auth.DTOs;
using MediatR;

namespace InlämningWebb1.Application.Features.Auth.Commands.Login;

/// <summary>
/// Handler for LoginCommand.
///
/// Flow:
///   1. Validate credentials via IUserService (returns null if wrong).
///   2. If valid, generate a signed JWT via ITokenService.
///   3. Return an AuthResponseDto containing the token and user info.
///
/// Both IUserService and ITokenService are interfaces defined in Application.
/// Their implementations live in Infrastructure — the handler knows nothing about
/// JWT libraries or where users are stored.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto?>
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    public Task<AuthResponseDto?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Check credentials — returns null if username or password is wrong
        var user = _userService.ValidateUser(request.Username, request.Password);
        if (user is null)
            return Task.FromResult<AuthResponseDto?>(null);

        // Step 2: Credentials are valid — generate a signed JWT token
        var token = _tokenService.GenerateToken(user.Id, user.Username, user.Role);

        // Step 3: Return the token and basic user info
        return Task.FromResult<AuthResponseDto?>(
            new AuthResponseDto(token, user.Username, user.Role));
    }
}
