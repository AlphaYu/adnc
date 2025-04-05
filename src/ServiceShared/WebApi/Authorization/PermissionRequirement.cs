namespace Adnc.Shared.WebApi.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement()
    {
    }

    public PermissionRequirement(string name) => Name = name;
    public string Name { get; init; } = string.Empty;
}
