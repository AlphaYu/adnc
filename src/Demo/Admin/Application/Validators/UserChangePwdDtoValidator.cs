using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="UserProfileChangePwdDto"/> instances.
/// </summary>
public class UserChangePwdDtoValidator : AbstractValidator<UserProfileChangePwdDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserChangePwdDtoValidator"/> class.
    /// </summary>
    public UserChangePwdDtoValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty().Length(5, User.Password_Maxlength);
        RuleFor(x => x.ConfirmPassword).NotEmpty().Length(5, User.Password_Maxlength)
                              .Must((dto, rePassword) =>
                              {
                                  return dto.NewPassword == rePassword;
                              })
                              .WithMessage("Confirm password must match the new password");
    }
}
