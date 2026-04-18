namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User.Validators;

/// <summary>
/// Validates <see cref="UserCreationAndUpdationDto"/> instances.
/// </summary>
public class UserCreationAndUpdationDtoValidator : AbstractValidator<UserCreationAndUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreationAndUpdationDtoValidator"/> class.
    /// </summary>
    public UserCreationAndUpdationDtoValidator()
    {
        RuleFor(x => x.Name).Required().Length(2, UserConsts.Name_Maxlength);
        RuleFor(x => x.Email).Required().MaximumLength(UserConsts.Email_Maxlength).EmailAddress();
        RuleFor(x => x.Mobile).Phone();
    }
}
