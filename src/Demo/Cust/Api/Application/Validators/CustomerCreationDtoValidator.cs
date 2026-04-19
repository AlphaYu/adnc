using Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer;

namespace Adnc.Demo.Cust.Api.Application.Validators;

public class CustomerCreationDtoValidator : AbstractValidator<CustomerCreationDto>
{
    public CustomerCreationDtoValidator()
    {
        RuleFor(x => x.Account).Length(2, Customer.Account_MaxLength).WithMessage("Account length must be between {MinLength} and {MaxLength}");
        RuleFor(x => x.Nickname).Length(2, Customer.Nickname_MaxLength).WithMessage("Nickname length must be between {MinLength} and {MaxLength}");
        RuleFor(x => x.Nickname).Length(2, Customer.Realname_Maxlength).WithMessage("Real name length must be between {MinLength} and {MaxLength}");
        //RuleFor(x => x.Password).Length(2, CustomerConsts.Password_Maxlength).WithMessage("Password length must be between {MinLength} and {MaxLength}");
    }
}
