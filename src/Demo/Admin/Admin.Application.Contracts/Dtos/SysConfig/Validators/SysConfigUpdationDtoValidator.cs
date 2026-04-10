namespace Adnc.Demo.Admin.Application.Contracts.Dtos.SysConfig.Validators;

public class SysConfigUpdationDtoValidator : AbstractValidator<SysConfigUpdationDto>
{
    public SysConfigUpdationDtoValidator()
    {
        Include(new SysConfigCreationDtoValidator());
    }
}
