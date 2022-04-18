using Adnc.Usr.Application.Contracts.Dtos;
using FluentValidation;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class RoleUpdationDtoValidator : AbstractValidator<RoleUpdationDto>
    {
        public RoleUpdationDtoValidator()
        {
            Include(new RoleCreationDtoValidator());
        }
    }
}