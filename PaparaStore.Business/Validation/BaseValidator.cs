using FluentValidation;
using PaparaStore.Base.Entity;

namespace PaparaStore.Business.Validation;
public class BaseValidator : AbstractValidator<BaseEntity>
{
    public BaseValidator()
    {
        RuleFor(x => x.InsertDate).NotNull();
        RuleFor(x => x.Id).NotNull();

    }
}
