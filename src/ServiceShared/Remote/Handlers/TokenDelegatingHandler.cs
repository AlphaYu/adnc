using Adnc.Shared.Remote.Handlers.Token;

namespace Adnc.Shared.Remote.Handlers;

/// <summary>
/// https://www.siakabaro.com/use-http-2-with-httpclient-in-net-6-0/
/// </summary>
public class TokenDelegatingHandler(TokenFactory tokenFactory) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (request.RequestUri.Scheme.EqualsIgnoreCase("https") && request.Version != new Version(2, 0))
        {
            request.Version = new Version(2, 0);
        }

        var headers = request.Headers;
        if (headers.Authorization is not null)
        {
            var authorizationScheme = headers.Authorization.Scheme;
            var tokenGenerator = tokenFactory.CreateGenerator(authorizationScheme);
            var tokenTxt = tokenGenerator.Create();
            headers.Authorization = new AuthenticationHeaderValue(authorizationScheme, tokenTxt);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
