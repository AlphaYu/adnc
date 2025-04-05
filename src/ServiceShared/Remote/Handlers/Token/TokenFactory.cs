namespace Adnc.Shared.Remote.Handlers.Token;

public sealed class TokenFactory(IEnumerable<ITokenGenerator> tokenGenerators)
{
    public ITokenGenerator CreateGenerator(string authorizationScheme)
    {
        /*
        if (authorizationScheme.EqualsIgnoreCase(BasicTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BasicTokenGenerator>(_provider);

        if (authorizationScheme.EqualsIgnoreCase(BearerTokenGenerator.Scheme))
            return ActivatorUtilities.CreateInstance<BearerTokenGenerator>(_provider);

        return default;
        */
        var tokenGenerator = tokenGenerators.First(x => x.GeneratorName == authorizationScheme);
        return tokenGenerator;
    }
}
