using FluentValidation;
using Simpra.Schema.UserRR;

namespace Simpra.Service.FluentValidation.User
{
    public class AppUserUpdateRequestValidator : AbstractValidator<AppUserUpdateRequest>
    {
        public AppUserUpdateRequestValidator()
        {
            RuleFor(x => x.UserName)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must be less than 31 character");

            RuleFor(x => x.FirstName)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must be less than 31 character");

            RuleFor(x => x.LastName)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must be less than 31 character");

            RuleFor(x => x.Email)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 101 character");

            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("{PropertyName} is required")
                .NotEmpty().WithMessage("{PropertyName} is required")
                .MaximumLength(30).WithMessage("{PropertyName} must be less than 31 character");

            RuleFor(x => x.DigitalWalletInformation)
                .MaximumLength(100).WithMessage("{PropertyName} must be less than 101 character");

            RuleFor(x => x.DigitalWalletBalance)
                .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} must be equal or greater than 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage("{PropertyName} must be less than (decimal.MaxValue)");
        }
    }
}
