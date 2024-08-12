using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class CartValidator : AbstractValidator<Cart>
{
    public CartValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.ProductAmount)
            .GreaterThan(0).WithMessage("Product Amount must be greater than zero.");

        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive status is required.");
    }
}
