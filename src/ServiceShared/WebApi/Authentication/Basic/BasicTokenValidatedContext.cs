namespace Adnc.Shared.WebApi.Authentication.Basic;

public class BasicTokenValidatedContext(HttpContext context, AuthenticationScheme scheme, BasicSchemeOptions options) : ResultContext<BasicSchemeOptions>(context, scheme, options)
{ }
