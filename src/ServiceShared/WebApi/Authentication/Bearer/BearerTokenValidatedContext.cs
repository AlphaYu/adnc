namespace Adnc.Shared.WebApi.Authentication.Bearer;

public class BearerTokenValidatedContext : ResultContext<BearerSchemeOptions>
{
    public BearerTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, BearerSchemeOptions options)
        : base(context, scheme, options)
    {
    }
}