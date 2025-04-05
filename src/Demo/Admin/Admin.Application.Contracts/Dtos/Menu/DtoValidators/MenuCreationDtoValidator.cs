namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class MenuCreationDtoValidator : AbstractValidator<MenuCreationDto>
{
    public MenuCreationDtoValidator()
    {
        RuleFor(x => x.Perm).MaximumLength(MenuConsts.Code_MaxLength);
        RuleFor(x => x.Name).NotEmpty().Length(2, MenuConsts.Name_MaxLength);
        RuleFor(x => x.Component).MaximumLength(MenuConsts.Component_MaxLength);
        RuleFor(x => x.Icon).MaximumLength(MenuConsts.Icon_MaxLength);
    }
}
