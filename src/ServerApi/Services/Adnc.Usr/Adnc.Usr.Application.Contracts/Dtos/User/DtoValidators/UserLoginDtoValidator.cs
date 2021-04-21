using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;
using Adnc.Core.Shared.EntityConsts.Usr;

namespace Adnc.Usr.Application.Contracts.DtoValidators
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
