using Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="RoleCreationDto"/> instances.
/// </summary>
public class RoleCreationDtoValidator : AbstractValidator<RoleCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleCreationDtoValidator"/> class.
    /// </summary>
    public RoleCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, Role.Name_MaxLength);
        RuleFor(x => x.Code).NotEmpty().Length(2, Role.Code_MaxLength);
    }
}
