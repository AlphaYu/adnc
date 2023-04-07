namespace Adnc.Shared.WebApi.Authorization;

public abstract class AbstractPermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not null && context.User.Identity.IsAuthenticated && context.Resource is HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith(Authentication.Basic.BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            var codes = httpContext.GetEndpoint()?.Metadata?.GetMetadata<AdncAuthorizeAttribute>()?.Codes;
            if (codes is null || codes.Length==0)
            {
                context.Succeed(requirement);
                return;
            }

            var userContext = httpContext.RequestServices.GetService<UserContext>() ?? throw new NullReferenceException(nameof(UserContext));
            var result = await CheckUserPermissions(userContext.Id, codes, userContext.RoleIds);
            if (result)
            {
                context.Succeed(requirement);
                return;
            }
        }
        context.Fail();
    }

    protected abstract Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds);
}