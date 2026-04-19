using Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="SysConfigCreationDto"/> instances.
/// </summary>
public class SysConfigCreationDtoValidator : AbstractValidator<SysConfigCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SysConfigCreationDtoValidator"/> class.
    /// </summary>
    public SysConfigCreationDtoValidator()
    {
        RuleFor(x => x.Key).NotEmpty().Length(2, SysConfig.Key_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, SysConfig.Name_MaxLength);
        RuleFor(x => x.Value).NotEmpty().Length(2, SysConfig.Value_MaxLength);
        RuleFor(x => x.Remark).MaximumLength(SysConfig.Remark_MaxLength);
    }
}
