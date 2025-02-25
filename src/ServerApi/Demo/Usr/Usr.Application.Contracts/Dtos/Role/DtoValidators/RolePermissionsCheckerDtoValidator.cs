namespace Adnc.Demo.Usr.Application.Contracts.DtoValidators;

public class RolePermissionsCheckerDtoValidator : AbstractValidator<RolePermissionsCheckerDto>
{
    public RolePermissionsCheckerDtoValidator()
    {
        RuleFor(x => x.RoleIds).NotNull().Must(x => x.Count() > 0);
        RuleFor(x => x.Permissions).NotNull().Must(x => x.Count() > 0);
    }
}