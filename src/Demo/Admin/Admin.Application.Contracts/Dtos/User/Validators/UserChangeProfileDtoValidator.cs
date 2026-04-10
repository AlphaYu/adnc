namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User.Validators;

public class UserChangeProfileDtoValidator : AbstractValidator<UserProfileUpdationDto>
{
    public UserChangeProfileDtoValidator()
    {
        RuleFor(x => x.Name).Length(2, UserConsts.Name_Maxlength);
    }
}
