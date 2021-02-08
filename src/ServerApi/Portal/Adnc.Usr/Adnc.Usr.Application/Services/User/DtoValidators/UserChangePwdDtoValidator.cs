using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserChangePwdDtoValidator : AbstractValidator<UserChangePwdDto>
    {
        public UserChangePwdDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().Length(5, 16);
            RuleFor(x => x.RePassword).NotEmpty().Length(5, 16)
                                      .Must((dto, rePassword) =>
                                      {
                                          return dto.Password == rePassword;
                                      }).WithMessage("重复密码必须跟新密码一样");
        }
    }
}
