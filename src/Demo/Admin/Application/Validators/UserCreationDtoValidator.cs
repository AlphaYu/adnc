using Adnc.Demo.Admin.Application.Contracts.Dtos.User;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="UserCreationDto"/> instances.
/// </summary>
public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserCreationDtoValidator"/> class.
    /// </summary>
    public UserCreationDtoValidator()
    {
        Include(new UserCreationAndUpdationDtoValidator());
        RuleFor(x => x.Account).Required().LetterNumberUnderscode().Length(5, User.Account_MaxLength);
        //RuleFor(x => x.Password).NotEmpty().When(x => x.Id < 1)
        //                        .Length(5, 16).When(x => x.Id < 1);
    }
}
