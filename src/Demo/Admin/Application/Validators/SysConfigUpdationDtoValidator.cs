using Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="SysConfigUpdationDto"/> instances.
/// </summary>
public class SysConfigUpdationDtoValidator : AbstractValidator<SysConfigUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SysConfigUpdationDtoValidator"/> class.
    /// </summary>
    public SysConfigUpdationDtoValidator()
    {
        Include(new SysConfigCreationDtoValidator());
    }
}
