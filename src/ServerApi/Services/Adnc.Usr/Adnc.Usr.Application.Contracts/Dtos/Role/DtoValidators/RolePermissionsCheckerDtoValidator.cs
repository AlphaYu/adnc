using FluentValidation;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class RolePermissionsCheckerDtoValidator : AbstractValidator<RolePermissionsCheckerDto>
    {
        public RolePermissionsCheckerDtoValidator()
        {
            RuleFor(x => x.RoleIds).NotNull().Must(x => x.Length > 0);
            RuleFor(x => x.Permissions).NotNull().Must(x => x.Length > 0);
        }
    }
}
