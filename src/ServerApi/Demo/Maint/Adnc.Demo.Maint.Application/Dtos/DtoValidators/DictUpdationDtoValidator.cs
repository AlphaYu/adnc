namespace Adnc.Demo.Maint.Application.Dtos.DtoValidators;

public class DictUpdationDtoValidator : AbstractValidator<DictUpdationDto>
{
    public DictUpdationDtoValidator()
    {
        Include(new DictCreationDtoValidator());
    }
}