using Adnc.Infra.Core.Interfaces;

namespace Adnc.Shared.Rpc.Handlers.Token;

public class BasicTokenGenerator : ITokenGenerator
{
    private readonly IServiceInfo _serviceInfo;

    public BasicTokenGenerator(IServiceInfo serviceInfo) => _serviceInfo = serviceInfo;

    public static string Scheme => "Basic";

    public virtual string Create() => this.PackToBase64(_serviceInfo.ShortName);
}