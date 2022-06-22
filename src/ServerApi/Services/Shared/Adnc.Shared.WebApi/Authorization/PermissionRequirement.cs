namespace Adnc.Shared.WebApi.Authentication;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; init; }

    public PermissionRequirement()
    {
    }

    public PermissionRequirement(string name) => Name = name;
}