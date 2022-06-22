namespace Adnc.Shared;

public sealed class UserContext
{
    public long Id { get; set; }
    public string Account { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string RoleIds { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    public string RemoteIpAddress { get; set; } = string.Empty;
}