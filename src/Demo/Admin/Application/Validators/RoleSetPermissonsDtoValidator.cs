using Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="RoleSetPermissonsDto"/> instances.
/// </summary>
public class RoleSetPermissonsDtoValidator : AbstractValidator<RoleSetPermissonsDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleSetPermissonsDtoValidator"/> class.
    /// </summary>
    public RoleSetPermissonsDtoValidator()
    {
        RuleFor(x => x.RoleId).GreaterThan(0);
        RuleFor(x => x.Permissions).NotNull();
    }
}
