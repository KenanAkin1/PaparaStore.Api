using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class CouponValidator : AbstractValidator<Coupon>
{
    public CouponValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Coupon Code is required.")
            .MaximumLength(50).WithMessage("Coupon Code cannot exceed 50 characters.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThan(0).WithMessage("Discount Amount must be greater than zero.");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.Now).WithMessage("End Date must be in the future.");

        RuleFor(x => x.CouponQuantity)
            .GreaterThan(0).WithMessage("Coupon Amount must be greater than zero.");
    }
}
