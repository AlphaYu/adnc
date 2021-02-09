using FluentValidation;
using Adnc.Cus.Application.Dtos;
using Adnc.Application.Shared.DtoValidators;

namespace Adnc.Cus.Application.DtoValidators
{
    public class RechargeInputDtoValidator : AbstractValidator<CustomerRechargeDto>
    {
        public RechargeInputDtoValidator()
        {
            RuleFor(x => x.Amount).NotEqual(0).WithMessage("充值金额不能等于{ComparisonValue}");
        }
    }
}
