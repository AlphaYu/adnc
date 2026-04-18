namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role.Validators;

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
        RuleFor(x => x.Name).NotEmpty().Length(2, RoleConsts.Name_MaxLength);
        RuleFor(x => x.Code).NotEmpty().Length(2, RoleConsts.Code_MaxLength);
    }
}
