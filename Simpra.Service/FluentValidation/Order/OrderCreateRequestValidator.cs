using FluentValidation;
using Simpra.Schema.OrderRR;
using Simpra.Schema.ProductRR;
using Simpra.Service.FluentValidation.CreditCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
