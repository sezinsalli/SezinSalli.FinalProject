using FluentValidation;
using Simpra.Schema.UserRR;

namespace Simpra.Service.FluentValidation
{
    public class AppUserCreateRequestValidator : AbstractValidator<AppUserCreateRequest>
    {
        public AppUserCreateRequestValidator()
        {
            RuleFor(x => x.UserName).NotNull().WithMessage("{UserName} is required")
                .NotEmpty().WithMessage("{UserName} is required");

            RuleFor(x => x.FirstName).NotNull().WithMessage("{FirstName} is required")
                .NotEmpty().WithMessage("{FirstName} is required");

            RuleFor(x => x.LastName).NotNull().WithMessage("{LastName} is required")
                .NotEmpty().WithMessage("{LastName} is required");

            RuleFor(x => x.Email).NotNull().WithMessage("{Email} is required")
                .NotEmpty().WithMessage("{Email} is required");

            RuleFor(x => x.Password).NotNull().WithMessage("{Password} is required")
                .NotEmpty().WithMessage("{Password} is required");

        }
    }
}
