using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserValidateInputDtoValidator : AbstractValidator<UserValidateInputDto>
    {
        public UserValidateInputDtoValidator()
        {
            RuleFor(x => x.Account).NotEmpty().Length(5,16);
            RuleFor(x => x.Password).NotEmpty().Length(5,16);
        }
    }
}
