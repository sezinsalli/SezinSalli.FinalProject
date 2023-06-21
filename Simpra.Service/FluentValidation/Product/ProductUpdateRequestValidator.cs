using FluentValidation;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.FluentValidation.Product
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} must be int value");

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .InclusiveBetween(0, int.MaxValue).WithMessage("{PropertyName} must be int value");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 101 character");

            RuleFor(x => x.Stock)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(int.MaxValue).WithMessage("{PropertyName} must be less than (int.MaxValue)");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage("{PropertyName} must be less than (decimal.MaxValue)");

            RuleFor(x => x.Property)
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 101 character");

            RuleFor(x => x.Definition)
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 101 character");

            RuleFor(x => x.Status)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.EarningPercentage)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(double.MaxValue).WithMessage("{PropertyName} must be less than (double.MaxValue)");

            RuleFor(x => x.MaxPuanAmount)
                .GreaterThan(0).WithMessage("{PropertyName} must be greater 0")
                .LessThanOrEqualTo(double.MaxValue).WithMessage("{PropertyName} must be less than (double.MaxValue)");

        }

    }
}
