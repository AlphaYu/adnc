namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class UserChangePwdDtoValidator : AbstractValidator<UserProfileChangePwdDto>
{
    public UserChangePwdDtoValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().Length(5, UserConsts.Password_Maxlength);
        RuleFor(x => x.ConfirmPassword).NotEmpty().Length(5, UserConsts.Password_Maxlength)
                                  .Must((dto, rePassword) =>
                                  {
                                      return dto.NewPassword == rePassword;
                                  })
                                  .WithMessage("重复密码必须跟新密码一样");
    }
}
