namespace Adnc.Shared.WebApi.Authentication.Basic;

public class BasicTokenValidatedContext : ResultContext<BasicSchemeOptions>
{
    public BasicTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, BasicSchemeOptions options)
        : base(context, scheme, options)
    {
    }
}