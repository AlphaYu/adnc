using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Shared.Rpc.Handlers.Token;

public sealed class TokenFactory
{
    private readonly IServiceProvider _provider;
    public TokenFactory(IServiceProvider provider) => _provider = provider;

    public ITokenGenerator? CreateGenerator(string authorizationScheme)
    {
        if (authorizationScheme.EqualsIgnoreCase(BasicTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BasicTokenGenerator>(_provider);

        if (authorizationScheme.EqualsIgnoreCase(BearerTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BearerTokenGenerator>(_provider);

        return default;
    }
}
