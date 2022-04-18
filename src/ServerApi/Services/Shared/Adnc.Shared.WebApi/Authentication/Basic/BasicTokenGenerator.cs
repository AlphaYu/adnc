using Adnc.Infra.Consul.TokenGenerator;

namespace Microsoft.AspNetCore.Authentication.Basic;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly IServiceInfo _serviceInfo;

    public BasicTokenGenerator(IServiceInfo serviceInfo) => _serviceInfo = serviceInfo;

    public string Scheme => BasicDefaults.AuthenticationScheme;

    public string Create() => BasicAuthenticationHandler.PackToBase64(_serviceInfo.ShortName);
}