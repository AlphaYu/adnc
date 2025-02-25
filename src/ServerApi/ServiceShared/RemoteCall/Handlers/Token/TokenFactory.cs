using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Shared.Rpc.Handlers.Token;

public sealed class TokenFactory
{
    private readonly IEnumerable<ITokenGenerator> _tokenGenerators;
    public TokenFactory(IEnumerable<ITokenGenerator> tokenGenerators) => _tokenGenerators = tokenGenerators;

    public ITokenGenerator CreateGenerator(string authorizationScheme)
    {
        /*
        if (authorizationScheme.EqualsIgnoreCase(BasicTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BasicTokenGenerator>(_provider);

        if (authorizationScheme.EqualsIgnoreCase(BearerTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BearerTokenGenerator>(_provider);

        return default;
        */
        var tokenGenerator = _tokenGenerators.FirstOrDefault(x => x.GeneratorName == authorizationScheme);
        if (tokenGenerator is null)
            throw new NullReferenceException(nameof(tokenGenerator));
        return tokenGenerator;
    }
}
