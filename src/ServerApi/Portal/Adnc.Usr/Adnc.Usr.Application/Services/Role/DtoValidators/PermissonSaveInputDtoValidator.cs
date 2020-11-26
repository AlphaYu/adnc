using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class PermissonSaveInputDtoValidator : AbstractValidator<PermissonSaveInputDto>
    {
        public PermissonSaveInputDtoValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.Permissions).NotNull();
        }
    }
}
