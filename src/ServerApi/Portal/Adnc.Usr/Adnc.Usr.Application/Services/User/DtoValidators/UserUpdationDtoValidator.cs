using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserUpdationDtoValidator : AbstractValidator<UserUpdationDto>
    {
        public UserUpdationDtoValidator()
        {
            Include(new UserCreationAndUpdationDtoValidator());
        }
    }
}
