namespace Adnc.Demo.Maint.Application.Dtos.DtoValidators;

public class CfgCreationDtoValidator : AbstractValidator<CfgCreationDto>
{
    public CfgCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, CfgConsts.Name_MaxLength);
        RuleFor(x => x.Value).NotEmpty().Length(2, CfgConsts.Value_MaxLength);
        RuleFor(x => x.Description).MaximumLength(CfgConsts.Description_MaxLength);
    }
}