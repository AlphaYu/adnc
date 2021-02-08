using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class RoleSetPermissonsDtoValidator : AbstractValidator<RoleSetPermissonsDto>
    {
        public RoleSetPermissonsDtoValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0);
            RuleFor(x => x.Permissions).NotNull();
        }
    }
}
