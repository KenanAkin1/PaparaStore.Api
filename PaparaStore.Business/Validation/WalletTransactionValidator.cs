using FluentValidation;
using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Validation;
public class WalletTransactionValidator : AbstractValidator<WalletTransaction>
{
    public WalletTransactionValidator()
    {
        RuleFor(x => x.WalletId)
            .NotEmpty().WithMessage("Wallet ID is required.");

        RuleFor(x => x.ReferenceNumber)
            .NotEmpty().WithMessage("Reference Number is required.");

        RuleFor(x => x.InitialAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Initial Amount cannot be negative.");

        RuleFor(x => x.SpentAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Spent Amount cannot be negative.");

        RuleFor(x => x.RemainingAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Remaining Amount cannot be negative.");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction Date is required.");
    }
}
