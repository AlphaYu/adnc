using Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="RoleUpdationDto"/> instances.
/// </summary>
public class RoleUpdationDtoValidator : AbstractValidator<RoleUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RoleUpdationDtoValidator"/> class.
    /// </summary>
    public RoleUpdationDtoValidator()
    {
        Include(new RoleCreationDtoValidator());
    }
}
