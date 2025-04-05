namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class DictUpdationDtoValidator : AbstractValidator<DictUpdationDto>
{
    public DictUpdationDtoValidator()
    {
        Include(new DictCreationDtoValidator());
    }
}

public class DictDataUpdationDtoValidator : AbstractValidator<DictDataUpdationDto>
{
    public DictDataUpdationDtoValidator()
    {
        Include(new DictDataCreationDtoValidator());
    }
}
