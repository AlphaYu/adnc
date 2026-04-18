namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization.Validators;

/// <summary>
/// Validates <see cref="OrganizationUpdationDto"/> instances.
/// </summary>
public class DeptUpdationDtoValidator : AbstractValidator<OrganizationUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeptUpdationDtoValidator"/> class.
    /// </summary>
    public DeptUpdationDtoValidator()
    {
        Include(new DeptCreationDtoValidator());
    }
}
