namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Menu.Validators;

public class MenuUpdationDtoValidator : AbstractValidator<MenuUpdationDto>
{
    public MenuUpdationDtoValidator()
    {
        Include(new MenuCreationDtoValidator());
    }
}
