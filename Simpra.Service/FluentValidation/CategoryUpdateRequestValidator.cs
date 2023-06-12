using FluentValidation;
using Simpra.Schema.CategoryRR;


namespace Simpra.Service.FluentValidation
{
    public class CategoryUpdateRequestValidator : AbstractValidator<CategoryUpdateRequest>
    {
        public CategoryUpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
            RuleFor(x => x.Id).InclusiveBetween(1, int.MaxValue).WithMessage("{Id} must be greater than 0");
        }
    }
}
