namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class SysConfigUpdationDtoValidator : AbstractValidator<SysConfigUpdationDto>
{
    public SysConfigUpdationDtoValidator()
    {
        Include(new SysConfigCreationDtoValidator());
    }
}
