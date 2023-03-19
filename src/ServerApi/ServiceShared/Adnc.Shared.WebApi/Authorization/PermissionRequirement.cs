namespace Adnc.Shared.WebApi.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; init; } = string.Empty;

    public PermissionRequirement()
    {
    }

    public PermissionRequirement(string name) => Name = name;
}