using Adnc.Demo.Admin.Application.Contracts.Dtos.Menu;

namespace Adnc.Demo.Admin.Application.Validators;

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
        RuleFor(x => x.Perm).MaximumLength(Menu.Code_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, Menu.Name_MaxLength);
        RuleFor(x => x.Component).MaximumLength(Menu.Component_MaxLength);
        RuleFor(x => x.Icon).MaximumLength(Menu.Icon_MaxLength);
    }
}
