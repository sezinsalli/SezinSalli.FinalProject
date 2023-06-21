using FluentValidation;
using Simpra.Schema.CreditCardRR;

namespace Simpra.Service.FluentValidation.CreditCard
{
    public class CreditCardRequestValidator : AbstractValidator<CreditCardRequest>
    {
        public CreditCardRequestValidator()
        {
            RuleFor(x => x.CardNumber)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Length(12).WithMessage("Length of {PropertyName} has to be 12");

            RuleFor(x => x.CVV)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Length(3).WithMessage("Length of {PropertyName} has to be 3");

            RuleFor(x => x.ExpiryMonth)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MinimumLength(1).WithMessage("Length of {PropertyName} has to be between 1 and 2")
                .MaximumLength(2).WithMessage("Length of {PropertyName} has to be between 1 and 2");

            RuleFor(x => x.ExpiryYear)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .Length(4).WithMessage("Length of {PropertyName} has to be 12");

        }
    }
}
