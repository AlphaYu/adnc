namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators;

public class RoleCreationDtoValidator : AbstractValidator<RoleCreationDto>
{
    public RoleCreationDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, RoleConsts.Name_MaxLength);
        RuleFor(x => x.Tips).NotEmpty().Length(2, RoleConsts.Tips_MaxLength);
    }
}