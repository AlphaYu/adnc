namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
{
    public UserCreationDtoValidator()
    {
        Include(new UserCreationAndUpdationDtoValidator());
        RuleFor(x => x.Account).Required().LetterNumberUnderscode().Length(5, UserConsts.Account_MaxLength);
        //RuleFor(x => x.Password).NotEmpty().When(x => x.Id < 1)
        //                        .Length(5, 16).When(x => x.Id < 1);
    }
}
