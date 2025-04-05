using Adnc.Demo.Const.Entity.Cust;

namespace Adnc.Demo.Cust.Api.Application.Dtos.DtoValidators;

public class CustomerCreationDtoValidator : AbstractValidator<CustomerCreationDto>
{
    public CustomerCreationDtoValidator()
    {
        RuleFor(x => x.Account).Length(2, CustomerConsts.Account_MaxLength).WithMessage("账号长度必须在{MinLength}到{MaxLength}之间");
        RuleFor(x => x.Nickname).Length(2, CustomerConsts.Nickname_MaxLength).WithMessage("昵称长度必须在{MinLength}到{MaxLength}之间");
        RuleFor(x => x.Nickname).Length(2, CustomerConsts.Realname_Maxlength).WithMessage("姓名长度必须在{MinLength}到{MaxLength}之间");
        //RuleFor(x => x.Password).Length(2, CustomerConsts.Password_Maxlength).WithMessage("密码长度必须在{MinLength}到{MaxLength}之间");
    }
}
