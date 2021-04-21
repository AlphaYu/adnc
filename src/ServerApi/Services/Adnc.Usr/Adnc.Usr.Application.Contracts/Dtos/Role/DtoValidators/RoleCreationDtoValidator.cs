using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class RoleCreationDtoValidator : AbstractValidator<RoleCreationDto>
    {
        public RoleCreationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 32);
            RuleFor(x => x.Tips).NotEmpty().Length(2, 64);
        }
    }
}
