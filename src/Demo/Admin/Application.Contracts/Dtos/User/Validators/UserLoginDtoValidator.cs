namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User.Validators;

/// <summary>
/// Validates <see cref="UserLoginDto"/> instances.
/// </summary>
public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginDtoValidator"/> class.
    /// </summary>
    public UserLoginDtoValidator()
    {
        RuleFor(x => x.Account).Required().Length(5, UserConsts.Account_MaxLength).LetterNumberUnderscode();
        RuleFor(x => x.Password).Required().Length(5, UserConsts.Password_Maxlength);
    }
}
