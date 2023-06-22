using FluentValidation;
using Simpra.Schema.BasketRR;

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
