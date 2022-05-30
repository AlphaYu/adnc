namespace Microsoft.AspNetCore.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; init; }

    public PermissionRequirement()
    {
    }

    public PermissionRequirement(string name) => Name = name;
}