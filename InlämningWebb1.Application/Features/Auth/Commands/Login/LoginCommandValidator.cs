using FluentValidation;

namespace InlämningWebb1.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validation rules for LoginCommand.
/// Runs automatically via ValidationBehavior before the handler executes.
/// Only validates that the fields are present — credential checking happens in the handler.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
