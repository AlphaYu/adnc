using Adnc.Demo.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Application.Contracts.Dtos.Customer.Validators;

public class CustomerCreationDtoValidator : AbstractValidator<CustomerCreationDto>
{
    public CustomerCreationDtoValidator()
    {
        RuleFor(x => x.Account).Length(2, CustomerConsts.Account_MaxLength).WithMessage("Account length must be between {MinLength} and {MaxLength}");
        RuleFor(x => x.Nickname).Length(2, CustomerConsts.Nickname_MaxLength).WithMessage("Nickname length must be between {MinLength} and {MaxLength}");
        RuleFor(x => x.Nickname).Length(2, CustomerConsts.Realname_Maxlength).WithMessage("Real name length must be between {MinLength} and {MaxLength}");
        //RuleFor(x => x.Password).Length(2, CustomerConsts.Password_Maxlength).WithMessage("Password length must be between {MinLength} and {MaxLength}");
    }
}
