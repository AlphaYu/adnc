namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Dict.Validators;

/// <summary>
/// Validates <see cref="DictUpdationDto"/> instances.
/// </summary>
public class DictUpdationDtoValidator : AbstractValidator<DictUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictUpdationDtoValidator"/> class.
    /// </summary>
    public DictUpdationDtoValidator()
    {
        Include(new DictCreationDtoValidator());
    }
}

/// <summary>
/// Validates <see cref="DictDataUpdationDto"/> instances.
/// </summary>
public class DictDataUpdationDtoValidator : AbstractValidator<DictDataUpdationDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DictDataUpdationDtoValidator"/> class.
    /// </summary>
    public DictDataUpdationDtoValidator()
    {
        Include(new DictDataCreationDtoValidator());
    }
}
