namespace Adnc.Maint.Application.Dtos.DtoValidators;

public class CfgUpdationDtoValidator : AbstractValidator<CfgUpdationDto>
{
    public CfgUpdationDtoValidator()
    {
        Include(new CfgCreationDtoValidator());
    }
}