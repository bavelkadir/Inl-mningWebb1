using InlämningWebb1.Application.Common.Models;

namespace InlämningWebb1.Application.Common.Interfaces;

/// <summary>
/// Contract for validating user credentials.
/// Defined in Application so the LoginCommandHandler can use it without
/// depending on the Infrastructure user store (hardcoded list, or later a database).
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Checks whether the username and password are valid.
    /// </summary>
    /// <returns>A UserRecord if credentials are correct, null if invalid.</returns>
    UserRecord? ValidateUser(string username, string password);
}
