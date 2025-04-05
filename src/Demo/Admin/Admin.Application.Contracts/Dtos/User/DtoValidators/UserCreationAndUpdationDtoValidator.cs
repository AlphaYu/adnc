namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class UserCreationAndUpdationDtoValidator : AbstractValidator<UserCreationAndUpdationDto>
{
    public UserCreationAndUpdationDtoValidator()
    {
        RuleFor(x => x.Name).Required().Length(2, UserConsts.Name_Maxlength);
        RuleFor(x => x.Email).Required().MaximumLength(UserConsts.Email_Maxlength).EmailAddress();
        RuleFor(x => x.Mobile).Phone();
    }
}
