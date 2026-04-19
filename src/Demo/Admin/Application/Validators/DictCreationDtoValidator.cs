using Adnc.Demo.Admin.Application.Contracts.Dtos.Dict;

namespace Adnc.Demo.Admin.Application.Validators;

/// <summary>
/// Validates <see cref="DictCreationDto"/> instances.
/// </summary>
public class DictCreationDtoValidator : AbstractValidator<DictCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictCreationDtoValidator"/> class.
    /// </summary>
    public DictCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, Dict.Name_MaxLength);
        RuleFor(x => x.Code).NotEmpty().Length(2, Dict.Code_MaxLength);
        RuleFor(x => x.Remark).MaximumLength(Dict.Remark_MaxLength);
    }
}

/// <summary>
/// Validates <see cref="DictDataCreationDto"/> instances.
/// </summary>
public class DictDataCreationDtoValidator : AbstractValidator<DictDataCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictDataCreationDtoValidator"/> class.
    /// </summary>
    public DictDataCreationDtoValidator()
    {
        RuleFor(x => x.DictCode).NotEmpty().Length(2, Dict.Code_MaxLength);
        RuleFor(x => x.Value).NotEmpty().MaximumLength(DictData.Value_MaxLength);
        RuleFor(x => x.Label).NotEmpty().MaximumLength(DictData.Label_MaxLength);
        RuleFor(x => x.TagType).MaximumLength(DictData.TagType_MaxLength);
    }
}
