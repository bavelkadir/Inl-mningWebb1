using FluentValidation;

namespace InlämningWebb1.Application.Features.Products.Commands.UpdateProduct;

/// <summary>
/// Validation rules for UpdateProductCommand.
/// Same field rules as Create, plus the Id must be a real (non-empty) GUID.
/// </summary>
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        // Id comes from the URL route — if it is Guid.Empty the request is malformed
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("A valid product ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("A valid CategoryId is required.");
    }
}
