using FluentValidation;
using Simpra.Schema.CouponRR;

namespace Simpra.Service.FluentValidation.Coupon
{
    public class CouponRequestValidator : AbstractValidator<CouponRequest>
    {
        public CouponRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.DiscountAmount)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage("{PropertyName} must be less than (decimal.MaxValue)");

            RuleFor(x => x.ExpirationDay)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("{PropertyName} must be less than (int.MaxValue)");
        }
    }
}
