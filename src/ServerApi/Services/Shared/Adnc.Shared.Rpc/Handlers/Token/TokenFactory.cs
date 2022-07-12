namespace Adnc.Shared.Rpc.Handlers.Token;

public sealed class TokenFactory
{
    private readonly IEnumerable<ITokenGenerator> _generators;
    public TokenFactory(IEnumerable<ITokenGenerator> generators) => _generators = generators;

    public ITokenGenerator? CreateGenerator(string authorizationScheme)
    {
        return _generators.FirstOrDefault(x => x.GeneratorName.EqualsIgnoreCase(authorizationScheme));
    }
}
