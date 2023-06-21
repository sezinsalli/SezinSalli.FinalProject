using FluentValidation;
using Simpra.Schema.BasketRR;
using Simpra.Schema.UserRR;
using Simpra.Service.FluentValidation.CreditCard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
