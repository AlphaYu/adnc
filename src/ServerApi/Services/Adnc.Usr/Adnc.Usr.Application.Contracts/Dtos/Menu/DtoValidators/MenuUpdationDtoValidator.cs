using Adnc.Usr.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class MenuUpdationDtoValidator : AbstractValidator<MenuUpdationDto>
    {
        public MenuUpdationDtoValidator()
        {
            Include(new MenuCreationDtoValidator());
        }
    }
}