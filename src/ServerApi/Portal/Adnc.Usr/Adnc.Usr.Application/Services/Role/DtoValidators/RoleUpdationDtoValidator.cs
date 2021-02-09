using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class RoleUpdationDtoValidator : AbstractValidator<RoleUpdationDto>
    {
        public RoleUpdationDtoValidator()
        {
            Include(new RoleCreationDtoValidator());
        }
    }
}
