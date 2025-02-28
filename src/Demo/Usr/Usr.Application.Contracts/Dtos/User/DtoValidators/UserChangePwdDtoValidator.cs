namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators
{
    public class UserChangePwdDtoValidator : AbstractValidator<UserChangePwdDto>
    {
        public UserChangePwdDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().Length(5, UserConsts.Password_Maxlength);
            RuleFor(x => x.RePassword).NotEmpty().Length(5, UserConsts.Password_Maxlength)
                                      .Must((dto, rePassword) =>
                                      {
                                          return dto.Password == rePassword;
                                      })
                                      .WithMessage("重复密码必须跟新密码一样");
        }
    }
}