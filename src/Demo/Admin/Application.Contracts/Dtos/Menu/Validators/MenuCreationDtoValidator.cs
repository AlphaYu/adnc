namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu.Validators;

/// <summary>
/// Validates <see cref="MenuCreationDto"/> instances.
/// </summary>
public class MenuCreationDtoValidator : AbstractValidator<MenuCreationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuCreationDtoValidator"/> class.
    /// </summary>
    public MenuCreationDtoValidator()
    {
        RuleFor(x => x.Perm).MaximumLength(MenuConsts.Code_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, MenuConsts.Name_MaxLength);
        RuleFor(x => x.Component).MaximumLength(MenuConsts.Component_MaxLength);
        RuleFor(x => x.Icon).MaximumLength(MenuConsts.Icon_MaxLength);
    }
}
