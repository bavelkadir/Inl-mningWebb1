namespace InlämningWebb1.Application.Common.Interfaces;

/// <summary>
/// Contract for generating JWT tokens.
/// Defined in Application so handlers can depend on it without knowing about
/// the JWT library — that knowledge belongs to Infrastructure.
/// </summary>
public interface ITokenService
{
    /// <summary>Generates a signed JWT token for the given user.</summary>
    /// <param name="userId">The user's unique identifier (embedded as 'sub' claim).</param>
    /// <param name="username">The username (embedded as 'unique_name' claim).</param>
    /// <param name="role">The user's role — 'Admin' or 'User' (embedded as 'role' claim).</param>
    /// <returns>A signed JWT string ready to be sent to the client.</returns>
    string GenerateToken(Guid userId, string username, string role);
}
