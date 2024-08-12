using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");

        RuleFor(x => x.ImageUrl)
            .MaximumLength(120).WithMessage("Image URL cannot exceed 120 characters.");
    }
}
