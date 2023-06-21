using FluentValidation;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.FluentValidation.Product
{
    public class ProductStockUpdateRequestValidator : AbstractValidator<ProductStockUpdateRequest>
    {
        public ProductStockUpdateRequestValidator()
        {
            RuleFor(x => x.Id)
               .NotNull().WithMessage("{PropertyName} is required")
               .NotEmpty().WithMessage("{PropertyName} is required");

            RuleFor(x => x.StockChange)
               .InclusiveBetween(int.MinValue, int.MaxValue).WithMessage("{PropertyName} must be int value");
        }
    }
}
