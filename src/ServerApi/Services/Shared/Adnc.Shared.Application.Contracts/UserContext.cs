namespace Adnc.Shared.Application.Contracts;

public class UserContext
{
    public string RemoteIpAddress { get; set; }

    public string Device { get; set; }

    public string Email { get; set; }

    public long[] RoleIds { get; set; }
    public long Id { get; set; }
    public string Account { get; set; }
    public string Name { get; set; }
}