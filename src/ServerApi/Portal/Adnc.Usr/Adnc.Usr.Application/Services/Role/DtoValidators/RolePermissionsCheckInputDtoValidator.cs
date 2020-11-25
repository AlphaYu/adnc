using FluentValidation;
using Adnc.Usr.Application.Dtos;

namespace Adnc.Usr.Application.DtoValidators
{
    public class RolePermissionsCheckInputDtoValidator : AbstractValidator<RolePermissionsCheckInputDto>
    {
        public RolePermissionsCheckInputDtoValidator()
        {
            RuleFor(x => x.RoleIds).NotNull().Must(x => x.Length > 0);
            RuleFor(x => x.Permissions).NotNull().Must(x => x.Length > 0);
        }
    }
}
