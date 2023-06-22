using FluentValidation;
using Simpra.Schema.BasketRR;

namespace Simpra.Service.FluentValidation.Basket
{
    public class BasketItemRequestValidator : AbstractValidator<BasketItemRequest>
    {
        public BasketItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} must be int value");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("{PropertyName} must be less than (int.MaxValue)");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage("{PropertyName} must be less than (decimal.MaxValue)");
        }
    }
}
