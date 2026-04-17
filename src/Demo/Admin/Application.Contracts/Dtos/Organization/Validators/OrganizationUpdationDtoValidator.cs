namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization.Validators;

/// <summary>
/// DeptUpdationDtoValidator
/// </summary>
public class DeptUpdationDtoValidator : AbstractValidator<OrganizationUpdationDto>
{
    /// <summary>
    /// DeptUpdationDtoValidator
    /// </summary>
    public DeptUpdationDtoValidator()
    {
        Include(new DeptCreationDtoValidator());
    }
}
