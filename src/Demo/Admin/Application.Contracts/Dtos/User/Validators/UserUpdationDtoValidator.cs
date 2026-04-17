namespace Adnc.Demo.Admin.Application.Contracts.Dtos.User.Validators;

public class UserUpdationDtoValidator : AbstractValidator<UserUpdationDto>
{
    public UserUpdationDtoValidator()
    {
        Include(new UserCreationAndUpdationDtoValidator());
    }
}
