using FluentValidation;
using Adnc.Usr.Application.Dtos;
using Adnc.Application.Shared.DtoValidators;
using Adnc.Usr.Core.Entities.Consts;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserCreationAndUpdationDtoValidator : AbstractValidator<UserCreationAndUpdationDto>
    {
        public UserCreationAndUpdationDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Matches(@"^[a-zA-Z][a-zA-Z0-9_]{4," + (UserConsts.Account_MaxLength - 1).ToString() + "}$").WithMessage("{PropertyName} 不合法,账号必须是5～16个字符,以字母开头,可包包含字母、数字、下划线。");
            RuleFor(x => x.Name).NotEmpty().Length(2, UserConsts.Name_Maxlength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(UserConsts.Email_Maxlength).EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(UserConsts.Phone_Maxlength).Phone();
            RuleFor(x => x.Birthday).NotEmpty();
        }
    }
}
