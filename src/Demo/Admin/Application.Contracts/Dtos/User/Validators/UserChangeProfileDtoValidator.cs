namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User.Validators;

/// <summary>
/// Validates <see cref="UserProfileUpdationDto"/> instances.
/// </summary>
public class UserChangeProfileDtoValidator : AbstractValidator<UserProfileUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserChangeProfileDtoValidator"/> class.
    /// </summary>
    public UserChangeProfileDtoValidator()
    {
        RuleFor(x => x.Name).Length(2, UserConsts.Name_Maxlength);
    }
}
