using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InlämningWebb1.Application.Common.Interfaces;
using InlämningWebb1.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace InlämningWebb1.Infrastructure.Services;

/// <summary>
/// Infrastructure implementation of ITokenService.
/// Uses System.IdentityModel.Tokens.Jwt to build and sign JWT tokens.
/// The Application layer never references this class directly — it depends only
/// on the ITokenService interface defined in Application.
/// </summary>
public class TokenService : ITokenService
{
    private readonly JwtSettings _settings;

    /// <summary>IOptions&lt;JwtSettings&gt; is populated from configuration (appsettings.json + User Secrets).</summary>
    public TokenService(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    /// <summary>
    /// Builds a signed JWT token containing the user's ID, username, role, and expiry.
    /// </summary>
    public string GenerateToken(Guid userId, string username, string role)
    {
        // Claims are the key-value pairs stored in the token's payload.
        // They identify the user without requiring a database lookup on each request.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,        userId.ToString()),   // subject = user ID
            new Claim(JwtRegisteredClaimNames.UniqueName, username),            // username
            new Claim(ClaimTypes.Role,                    role),                // used by [Authorize(Roles=)]
            new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()) // unique token ID
        };

        // The signing key must match what the JWT Bearer middleware uses to validate tokens.
        // Both sides use the same key — this is HMAC (symmetric) signing.
        var keyBytes = Encoding.UTF8.GetBytes(_settings.Key);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // Build the token with all its parts
        var token = new JwtSecurityToken(
            issuer:             _settings.Issuer,
            audience:           _settings.Audience,
            claims:             claims,
            expires:            DateTime.UtcNow.AddMinutes(_settings.ExpiresInMinutes),
            signingCredentials: credentials);

        // Serialize the token to its compact string representation (the three-part dot-separated string)
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
