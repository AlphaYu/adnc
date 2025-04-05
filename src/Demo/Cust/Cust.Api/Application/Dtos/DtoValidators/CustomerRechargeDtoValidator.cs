namespace Adnc.Demo.Cust.Api.Application.Dtos.DtoValidators;

public class CustomerRechargeDtoValidator : AbstractValidator<CustomerRechargeDto>
{
    public CustomerRechargeDtoValidator()
    {
        RuleFor(x => x.Amount).NotEqual(0).WithMessage("充值金额不能等于{ComparisonValue}");
    }
}
