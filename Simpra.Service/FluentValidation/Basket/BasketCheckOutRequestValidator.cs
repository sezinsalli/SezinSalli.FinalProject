using FluentValidation;
using Simpra.Schema.BasketRR;
using Simpra.Service.FluentValidation.CreditCard;

namespace Simpra.Service.FluentValidation.Basket
{
    public class BasketCheckOutRequestValidator : AbstractValidator<BasketCheckOutRequest>
    {
        public BasketCheckOutRequestValidator()
        {
            RuleFor(x => x.CouponCode)
                .MaximumLength(10).WithMessage("{PropertyName} must be less than 11 character");

            RuleFor(x => x.CreditCard).SetValidator(new CreditCardRequestValidator());
        }
    }
}
