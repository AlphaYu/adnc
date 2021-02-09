using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Length(5,16);
            RuleFor(x => x.Password).NotEmpty().Length(5,16);
        }
    }
}
