using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class MenuUpdationDtoValidator : AbstractValidator<MenuUpdationDto>
    {
        public MenuUpdationDtoValidator()
        {
            Include(new MenuCreationDtoValidator());
        }
    }
}
