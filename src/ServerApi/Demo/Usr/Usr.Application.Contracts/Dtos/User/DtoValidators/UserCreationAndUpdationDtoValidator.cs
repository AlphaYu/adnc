namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators
{
    public class UserCreationAndUpdationDtoValidator : AbstractValidator<UserCreationAndUpdationDto>
    {
        public UserCreationAndUpdationDtoValidator()
        {
            RuleFor(x => x.Account).Required().LetterNumberUnderscode().Length(4, UserConsts.Account_MaxLength);
            RuleFor(x => x.Name).Required().Length(2, UserConsts.Name_Maxlength);
            RuleFor(x => x.Email).Required().MaximumLength(UserConsts.Email_Maxlength).EmailAddress();
            RuleFor(x => x.Phone).Phone();
            RuleFor(x => x.Birthday).Required();
        }
    }
}