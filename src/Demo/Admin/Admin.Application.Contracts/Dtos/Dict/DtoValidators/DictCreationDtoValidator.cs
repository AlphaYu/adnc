namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class DictCreationDtoValidator : AbstractValidator<DictCreationDto>
{
    public DictCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, DictConsts.Name_MaxLength);
        RuleFor(x => x.Code).NotEmpty().Length(2, DictConsts.Code_MaxLength);
        RuleFor(x => x.Remark).MaximumLength(DictConsts.Remark_MaxLength);
    }
}

public class DictDataCreationDtoValidator : AbstractValidator<DictDataCreationDto>
{
    public DictDataCreationDtoValidator()
    {
        RuleFor(x => x.DictCode).NotEmpty().Length(2, DictConsts.Code_MaxLength);
        RuleFor(x => x.Value).NotEmpty().MaximumLength(DictDataConsts.Value_MaxLength);
        RuleFor(x => x.Label).NotEmpty().MaximumLength(DictDataConsts.Label_MaxLength);
        RuleFor(x => x.TagType).MaximumLength(DictDataConsts.TagType_MaxLength);
    }
}
