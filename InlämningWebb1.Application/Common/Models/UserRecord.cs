namespace InlämningWebb1.Application.Common.Models;

/// <summary>
/// Represents a validated user returned from IUserService.
/// Contains only the data the Application layer needs — no password.
/// This is NOT a domain entity and is NOT stored in the database.
/// </summary>
public record UserRecord(Guid Id, string Username, string Role);
