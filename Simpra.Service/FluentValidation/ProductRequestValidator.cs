using FluentValidation;
using Simpra.Schema.ProductRR;

namespace Simpra.Service.FluentValidation
{
    public class ProductRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductRequestValidator()
        {
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage("{Id} must be greater than 0");
            RuleFor(x => x.Name).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
        }
    }
}
