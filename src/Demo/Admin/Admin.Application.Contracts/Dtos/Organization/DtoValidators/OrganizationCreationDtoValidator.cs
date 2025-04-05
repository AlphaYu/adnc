namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

/// <summary>
/// DeptCreationDto
/// </summary>
public class DeptCreationDtoValidator : AbstractValidator<OrganizationCreationDto>
{
    /// <summary>
    /// DeptCreationDtoValidator
    /// </summary>
    public DeptCreationDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Length(2, DeptConsts.Code_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, DeptConsts.Name_MaxLength);
        //RuleFor(x => x.MenuPerm).NotEmpty().MaximumLength(64);
        //RuleFor(x => x.ParentId).GreaterThan(1).WithMessage("{PropertyName} 不能为空")
        //                                  .NotEqual(x => x.Id).When(x => x.Id > 0);

        RuleFor(x => x.ParentId).GreaterThan(-1).WithMessage("{PropertyName} 不能为空");
    }
}
