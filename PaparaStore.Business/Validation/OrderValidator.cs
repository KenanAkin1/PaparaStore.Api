using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("Order Number is required.");

        RuleFor(x => x.TotalPrice)
            .GreaterThan(0).WithMessage("Total Price must be greater than zero.");

        RuleFor(x => x.FinalPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Final Price cannot be negative.");

        RuleFor(x => x.TotalRewardAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Total Reward Amount cannot be negative.");
    }
}
