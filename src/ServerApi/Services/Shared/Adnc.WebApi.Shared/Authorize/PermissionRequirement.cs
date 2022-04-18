namespace Microsoft.AspNetCore.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string _permissionName { get; }

        public PermissionRequirement() 
        { 
        }

        public PermissionRequirement(string permissionName)
            =>_permissionName = permissionName;
    }
}