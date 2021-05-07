using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using Adnc.Usr.Application.Contracts.Dtos;

namespace Adnc.Usr.Application.Contracts.DtoValidators
{
    public class RolePermissionsCheckerDtoValidator : AbstractValidator<RolePermissionsCheckerDto>
    {
        public RolePermissionsCheckerDtoValidator()
        {
            RuleFor(x => x.RoleIds).NotNull().Must(x=>x.Count()>0);
            RuleFor(x => x.Permissions).NotNull().Must(x => x.Count() > 0);
        }
    }
}
