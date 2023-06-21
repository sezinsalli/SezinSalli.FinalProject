using FluentValidation;
using Simpra.Schema.OrderRR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpra.Service.FluentValidation.Order
{
    public class OrderDetailRequestValidator : AbstractValidator<OrderDetailRequest>
    {
        public OrderDetailRequestValidator()
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
