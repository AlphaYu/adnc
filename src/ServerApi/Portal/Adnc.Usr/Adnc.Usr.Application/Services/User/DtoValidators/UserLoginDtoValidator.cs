using FluentValidation;
using Adnc.Usr.Application.Dtos;
using Adnc.Usr.Core.Entities.Consts;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Length(5, UserConsts.Account_MaxLength);
            RuleFor(x => x.Password).NotEmpty().Length(5, UserConsts.Password_Maxlength);
        }
    }
}
