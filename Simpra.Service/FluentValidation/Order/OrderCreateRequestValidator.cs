using FluentValidation;
using Simpra.Schema.OrderRR;
using Simpra.Service.FluentValidation.CreditCard;

namespace Simpra.Service.FluentValidation.Order
{
    public class OrderCreateRequestValidator : AbstractValidator<OrderCreateRequest>
    {
        public OrderCreateRequestValidator()
        {
            RuleFor(x => x.CouponCode)
                .MaximumLength(10).WithMessage("{PropertyName} must be less than 11 character");

            RuleFor(x => x.CreditCard).SetValidator(new CreditCardRequestValidator());

            RuleForEach(x => x.OrderDetails).SetValidator(new OrderDetailRequestValidator());
        }
    }
}
