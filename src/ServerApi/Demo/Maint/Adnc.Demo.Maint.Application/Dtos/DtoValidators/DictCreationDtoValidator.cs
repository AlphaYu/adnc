namespace Adnc.Demo.Maint.Application.Dtos.DtoValidators;

public class DictCreationDtoValidator : AbstractValidator<DictCreationDto>
{
    public DictCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(DictConsts.Name_MaxLength);
        RuleFor(x => x.Value).MaximumLength(DictConsts.Value_MaxLength);
    }
}