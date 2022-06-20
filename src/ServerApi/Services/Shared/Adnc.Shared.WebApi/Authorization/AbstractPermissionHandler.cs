using Adnc.Shared.WebApi.Authentication.Basic;

namespace Adnc.Shared.WebApi.Authentication;

public abstract class AbstractPermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity.IsAuthenticated && context.Resource is HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith(BasicDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
                return;
            }

            var userId = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId).Value.ToLong().Value;
            var validationVersion = context.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var codes = httpContext.GetEndpoint().Metadata.GetMetadata<PermissionAttribute>().Codes;
            var result = await CheckUserPermissions(userId, codes, validationVersion);
            if (result)
            {
                context.Succeed(requirement);
                return;
            }
        }
        context.Fail();
    }

    protected abstract Task<bool> CheckUserPermissions(long userId, IEnumerable<string> codes, string validationVersion);
}