namespace Adnc.Shared.ResultModels;

public class UserContext : IUserContext
{
    public string RemoteIpAddress { get; set; }

    public string Device { get; set; }

    public string Email { get; set; }

    public long[] RoleIds { get; set; }
    public long Id { get; set; }
    public string Account { get; set; }
    public string Name { get; set; }
}