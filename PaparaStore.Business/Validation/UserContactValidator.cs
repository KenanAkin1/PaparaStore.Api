using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class UserContactValidator : AbstractValidator<UserContact>
{
    public UserContactValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country Code is required.")
            .MaximumLength(3).WithMessage("Country Code cannot exceed 3 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .MaximumLength(10).WithMessage("Phone number cannot exceed 10 digits.");

        RuleFor(x => x.IsDefault)
            .NotNull().WithMessage("IsDefault status is required.");
    }
}
