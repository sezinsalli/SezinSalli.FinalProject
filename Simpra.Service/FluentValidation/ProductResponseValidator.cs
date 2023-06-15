using FluentValidation;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.FluentValidation
{
    public class ProductResponseValidator : AbstractValidator<ProductResponse>
    {
        public ProductResponseValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
            RuleFor(x => x.Stock).NotNull().WithMessage("{Stock} is required").NotEmpty().WithMessage("{Stock} is required");
            RuleFor(x => x.Price).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");
            RuleFor(x => x.Property).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");
            RuleFor(x => x.Definition).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");
            RuleFor(x => x.IsActive).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");
            RuleFor(x => x.EarningPercentage).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");
            RuleFor(x => x.MaxPuanAmount).NotNull().WithMessage("{Price} is required").NotEmpty().WithMessage("{Price} is required");

        }
    }
}
