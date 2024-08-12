using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Stock Quantity cannot be negative.");

        RuleFor(x => x.RewardPercantage)
            .InclusiveBetween(0, 100).WithMessage("Reward Percentage must be between 0 and 100.");

        RuleFor(x => x.MaxRewardAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Max Reward Amount cannot be negative.");
    }
}
