namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Account).Required().Length(5, UserConsts.Account_MaxLength).LetterNumberUnderscode();
            RuleFor(x => x.Password).Required().Length(5, UserConsts.Password_Maxlength);
        }
    }
}