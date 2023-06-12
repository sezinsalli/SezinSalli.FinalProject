using FluentValidation;
using Simpra.Schema.CategoryRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.FluentValidation
{
    public class CategoryResponseValidator : AbstractValidator<CategoryResponse>
    {
        public CategoryResponseValidator()
        {
            RuleFor(x => x.Name).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
            RuleFor(x => x.Tag).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
            RuleFor(x => x.Url).NotNull().WithMessage("{Name} is required").NotEmpty().WithMessage("{Name} is required");
        }
    }
}
