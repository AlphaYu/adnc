namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Organization.Validators;

/// <summary>
/// Validates <see cref="OrganizationCreationDto"/> instances.
/// </summary>
public class DeptCreationDtoValidator : AbstractValidator<OrganizationCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeptCreationDtoValidator"/> class.
    /// </summary>
    public DeptCreationDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Length(2, DeptConsts.Code_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, DeptConsts.Name_MaxLength);
        //RuleFor(x => x.MenuPerm).NotEmpty().MaximumLength(64);
        //RuleFor(x => x.ParentId).GreaterThan(1).WithMessage("{PropertyName} cannot be empty.")
        //                                  .NotEqual(x => x.Id).When(x => x.Id > 0);

        RuleFor(x => x.ParentId).GreaterThan(-1).WithMessage("{PropertyName} cannot be empty");
    }
}
