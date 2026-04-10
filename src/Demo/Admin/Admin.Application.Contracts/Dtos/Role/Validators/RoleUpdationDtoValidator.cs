namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role.Validators;

public class RoleUpdationDtoValidator : AbstractValidator<RoleUpdationDto>
{
    public RoleUpdationDtoValidator()
    {
        Include(new RoleCreationDtoValidator());
    }
}
