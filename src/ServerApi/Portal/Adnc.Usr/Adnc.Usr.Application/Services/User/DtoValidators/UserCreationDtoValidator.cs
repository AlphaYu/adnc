using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class UserCreationDtoValidator : AbstractValidator<UserCreationDto>
    {
        public UserCreationDtoValidator()
        {
            Include(new UserCreationAndUpdationDtoValidator());
            RuleFor(x => x.Password).NotEmpty().Length(5, 16);
            //RuleFor(x => x.Password).NotEmpty().When(x => x.Id < 1)
            //                        .Length(5, 16).When(x => x.Id < 1);
        }
    }
}
