namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators;

public class MenuCreationDtoValidator : AbstractValidator<MenuCreationDto>
{
    public MenuCreationDtoValidator()
    {
        RuleFor(x => x.Code).NotEmpty().Length(2, MenuConsts.Code_MaxLength);
        RuleFor(x => x.PCode).MaximumLength(MenuConsts.PCode_MaxLength).NotEqual(x => x.Code).When(x => x.PCode.IsNotNullOrWhiteSpace());
        RuleFor(x => x.Name).NotEmpty().Length(2, MenuConsts.Name_MaxLength);
        RuleFor(x => x.Url).NotEmpty().MaximumLength(MenuConsts.Url_MaxLength);
        RuleFor(x => x.Component).MaximumLength(MenuConsts.Component_MaxLength);
        RuleFor(x => x.Icon).MaximumLength(MenuConsts.Icon_MaxLength);
    }
}