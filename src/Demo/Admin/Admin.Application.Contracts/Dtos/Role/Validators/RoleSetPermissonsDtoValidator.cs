namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role.Validators;

public class RoleSetPermissonsDtoValidator : AbstractValidator<RoleSetPermissonsDto>
{
    public RoleSetPermissonsDtoValidator()
    {
        RuleFor(x => x.RoleId).GreaterThan(0);
        RuleFor(x => x.Permissions).NotNull();
    }
}
