using Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;

namespace Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer.Validators;

public class CustomerRechargeDtoValidator : AbstractValidator<CustomerRechargeDto>
{
    public CustomerRechargeDtoValidator()
    {
        RuleFor(x => x.Amount).NotEqual(0).WithMessage("Recharge amount cannot equal {ComparisonValue}");
    }
}
