using FluentValidation;

namespace InlämningWebb1.Application.Features.Products.Commands.CreateProduct;

/// <summary>
/// Validation rules for CreateProductCommand.
/// Discovered and registered automatically by AddValidatorsFromAssembly().
/// Runs via ValidationBehavior before CreateProductHandler executes.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        // Name is mandatory and must fit a reasonable length for a database column
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        // A product cannot be free or negatively priced
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        // Every product must belong to a category — Guid.Empty means no category was provided
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("A valid CategoryId is required.");
    }
}
