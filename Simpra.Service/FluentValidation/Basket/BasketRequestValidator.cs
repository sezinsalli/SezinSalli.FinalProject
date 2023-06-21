using FluentValidation;
using Simpra.Schema.BasketRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.FluentValidation.Basket
{
    public class BasketRequestValidator : AbstractValidator<BasketRequest>
    {
        public BasketRequestValidator()
        {
            RuleForEach(x => x.BasketItems).SetValidator(new BasketItemRequestValidator());
        }
    }
}
