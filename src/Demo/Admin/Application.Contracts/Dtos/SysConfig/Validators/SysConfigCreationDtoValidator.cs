namespace Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig.Validators;

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
        RuleFor(x => x.Key).NotEmpty().Length(2, SysConfigConsts.Key_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, SysConfigConsts.Name_MaxLength);
        RuleFor(x => x.Value).NotEmpty().Length(2, SysConfigConsts.Value_MaxLength);
        RuleFor(x => x.Remark).MaximumLength(SysConfigConsts.Remark_MaxLength);
    }
}
