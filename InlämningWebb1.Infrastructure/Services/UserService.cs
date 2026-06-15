using InlämningWebb1.Application.Common.Interfaces;
using InlämningWebb1.Application.Common.Models;

namespace InlämningWebb1.Infrastructure.Services;

/// <summary>
/// Simple in-memory user store for this school assignment.
/// Passwords are stored as plaintext — acceptable here for simplicity.
/// In production: store users in a database with hashed passwords (bcrypt / Argon2).
/// Test credentials are documented in README.md.
/// </summary>
public class UserService : IUserService
{
    // Internal record — never exposed outside this class.
    // Keeps credentials separate from the public UserRecord returned to callers.
    private sealed record StoredUser(Guid Id, string Username, string Password, string Role);

    // Fixed GUIDs make token sub claims consistent between restarts (good for testing)
    private static readonly List<StoredUser> _users =
    [
        new(Guid.Parse("11111111-1111-1111-1111-111111111111"), "admin", "Admin123!", "Admin"),
        new(Guid.Parse("22222222-2222-2222-2222-222222222222"), "user",  "User123!",  "User")
    ];

    /// <summary>
    /// Validates the username and password.
    /// Returns a UserRecord (no password) if valid, null if credentials are wrong.
    /// Case-sensitive username comparison — "Admin" ≠ "admin".
    /// </summary>
    public UserRecord? ValidateUser(string username, string password)
    {
        var found = _users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);

        return found is null
            ? null
            : new UserRecord(found.Id, found.Username, found.Role);
    }
}
