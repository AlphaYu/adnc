using Adnc.Shared.Rpc.Handlers.Token;

namespace Adnc.Shared.Rpc.Handlers;

/// <summary>
/// https://www.siakabaro.com/use-http-2-with-httpclient-in-net-6-0/
/// </summary>
public class TokenDelegatingHandler : DelegatingHandler
{
    private readonly TokenFactory _tokenFactory;

    public TokenDelegatingHandler(TokenFactory tokenFactory) => _tokenFactory = tokenFactory;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(request.RequestUri is null)
            throw new ArgumentNullException(nameof(request));

        if (request.RequestUri.Scheme.EqualsIgnoreCase("https") && request.Version != new Version(2, 0))
            request.Version = new Version(2, 0);

        var headers = request.Headers;
        if (headers.Authorization is not null)
        {
            var authorizationScheme = headers.Authorization.Scheme;
            var tokenGenerator = _tokenFactory.CreateGenerator(authorizationScheme);
            var tokenTxt = tokenGenerator.Create();
            headers.Authorization = new AuthenticationHeaderValue(authorizationScheme, tokenTxt);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}