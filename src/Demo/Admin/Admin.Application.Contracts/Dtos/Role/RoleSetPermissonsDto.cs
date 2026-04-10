namespace Adnc.Demo.Admin.Application.Contracts.Dtos.Role;

public class RoleSetPermissonsDto : IDto
{
    public long RoleId { set; get; }
    public long[] Permissions { get; set; } = [];
}
