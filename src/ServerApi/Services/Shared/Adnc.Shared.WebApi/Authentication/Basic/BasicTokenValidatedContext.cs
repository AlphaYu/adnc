namespace Microsoft.AspNetCore.Authentication.Basic;

 public class BasicTokenValidatedContext : ResultContext<BasicSchemeOptions>
{
    public BasicTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, BasicSchemeOptions options)
        : base(context, scheme, options)
    {
    }
}