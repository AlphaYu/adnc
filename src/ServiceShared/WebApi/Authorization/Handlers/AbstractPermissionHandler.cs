using System.Text.RegularExpressions;

namespace Adnc.Shared.WebApi.Authorization.Handlers;

public abstract class AbstractPermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is null || context.User.Identity.IsAuthenticated == false)
        {
            context.Fail(new AuthorizationFailureReason(this, "context.User.Identity is null || context.User.Identity.IsAuthenticated == false"));
            return;
        }

        if (context.Resource is HttpContext httpContext == false)
        {
            context.Fail(new AuthorizationFailureReason(this, "context.Resource is not HttpContext"));
            return;
        }

        var authHeader = httpContext.Request.Headers.Authorization.ToString();
        if (authHeader is not null && authHeader.StartsWith(Authentication.Basic.BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
            return;
        }

        var codes = httpContext.GetEndpoint()?.Metadata?.GetMetadata<AdncAuthorizeAttribute>()?.Codes;
        // for cap start
        if (codes is null || codes.Length == 0)
        {
            var perm = httpContext.Request.Query["perm"].ToString();
            if (perm.IsNotNullOrWhiteSpace())
            {
                codes = [perm];
            }
            else
            {
                var refererUrl = httpContext.Request.Headers.Referer.ToString();
                if (!string.IsNullOrEmpty(refererUrl))
                {
                    var pattern = @"\?perm=([^&]+)";
                    var match = Regex.Match(refererUrl, pattern);
                    if (match.Success)
                    {
                        codes = [match.Groups[1].Value];
                    }
                }
            }
        }
        // for cap end
        if (codes is null || codes.Length == 0)
        {
            context.Fail();
            return;
        }
        else
        {
            var userContext = httpContext.RequestServices.GetRequiredService<UserContext>();
            var result = await CheckUserPermissions(userContext.Id, codes, userContext.RoleIds);
            if (result)
            {
                context.Succeed(requirement);
                return;
            }
            else
            {
                context.Fail();
                return;
            }
        }
    }

    protected abstract Task<bool> CheckUserPermissions(long userId, IEnumerable<string> requestPermissions, string userBelongsRoleIds);
}
