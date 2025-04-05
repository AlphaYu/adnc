namespace Adnc.Demo.Admin.Application.Contracts.DtoValidators;

public class RoleCreationDtoValidator : AbstractValidator<RoleCreationDto>
{
    public RoleCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, RoleConsts.Name_MaxLength);
        RuleFor(x => x.Code).NotEmpty().Length(2, RoleConsts.Code_MaxLength);
    }
}
