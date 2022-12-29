namespace Adnc.Usr.Application.Contracts.DtoValidators;

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