using InlämningWebb1.Application.Common.Interfaces;
using InlämningWebb1.Application.Common.Models;

namespace InlämningWebb1.Infrastructure.Services;

/// <summary>
/// Simple in-memory user store for this school assignment.
///
/// IMPORTANT — this is intentionally simplified:
///   - Users are hardcoded (no database table).
///   - Passwords are stored as plaintext (never do this in production).
///   - In a real application: store users in the database, hash passwords
///     with bcrypt or Argon2, and use a proper user management solution.
///
/// Two accounts are pre-configured:
///   admin / Admin123!  → Role: Admin  (can read AND write products)
///   user  / User123!   → Role: User   (can only read products)
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
