using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Application.Validators;

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
        RuleFor(x => x.Name).Required().Length(2, User.Name_Maxlength);
        RuleFor(x => x.Email).Required().MaximumLength(User.Email_Maxlength).EmailAddress();
        RuleFor(x => x.Mobile).Phone();
    }
}
