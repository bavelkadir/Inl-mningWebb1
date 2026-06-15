namespace InlämningWebb1.Infrastructure.Settings;

/// <summary>
/// Strongly-typed representation of the "Jwt" section in configuration.
/// Non-secret values (Issuer, Audience, ExpiresInMinutes) come from appsettings.json.
/// The signing Key comes from User Secrets in development (never from appsettings.json).
/// Populated automatically by services.Configure&lt;JwtSettings&gt;().
/// </summary>
public class JwtSettings
{
    /// <summary>The configuration section name — matches the key in appsettings.json.</summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// The secret signing key. Must be at least 32 characters (256 bits) for HS256.
    /// Loaded from User Secrets in development; from environment variables in production.
    /// NEVER commit this value to source control.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>The token issuer — identifies who created the token.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>The token audience — identifies who the token is intended for.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>How long (in minutes) the token is valid before it expires.</summary>
    public int ExpiresInMinutes { get; set; } = 60;
}
