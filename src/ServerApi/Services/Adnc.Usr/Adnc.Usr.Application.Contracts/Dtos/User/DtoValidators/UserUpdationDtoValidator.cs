using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class UserUpdationDtoValidator : AbstractValidator<UserUpdationDto>
    {
        public UserUpdationDtoValidator()
        {
            Include(new UserCreationAndUpdationDtoValidator());
        }
    }
}
