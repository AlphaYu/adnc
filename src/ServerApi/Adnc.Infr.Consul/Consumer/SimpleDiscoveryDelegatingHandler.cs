using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Adnc.Infr.Consul.Consumer
{
    public class SimpleDiscoveryDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMemoryCache _memoryCache;
        public Guid ContextId { get; private set; }

        public SimpleDiscoveryDelegatingHandler(ITokenGenerator tokenGenerator
            , IMemoryCache memoryCache)
        {
            this.ContextId = Guid.NewGuid();
            _tokenGenerator = tokenGenerator;
            _memoryCache = memoryCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = request.Headers;

            var auth = headers.Authorization;
            if (auth != null)
            {
                var tokenTxt = _tokenGenerator?.Create();

                if (!string.IsNullOrEmpty(tokenTxt))
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }

            if (request.Method == HttpMethod.Get)
            {
                var cache = headers.FirstOrDefault(x => x.Key == "Cache");

                if (!string.IsNullOrWhiteSpace(cache.Key))
                {
                    int.TryParse(cache.Value.FirstOrDefault(), out int milliseconds);

                    if (milliseconds > 0)
                    {
                        var cacheKey = request.RequestUri.AbsoluteUri.GetHashCode();

                        var existCache = _memoryCache.TryGetValue(cacheKey, out string content);
                        if (existCache)
                        {
                            var resp = new HttpResponseMessage
                            {
                                Content = new StringContent(content, Encoding.UTF8)
                            };

                            return resp.EnsureSuccessStatusCode();
                        }

                        //SendAsync异常(请求、超时异常)，会throw
                        //服务端异常，不会抛出
                        var responseResult = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                        if (responseResult.IsSuccessStatusCode)
                            _memoryCache.Set(cacheKey, await responseResult.Content.ReadAsStringAsync());

                        return responseResult;
                    }
                }
            }


            var result = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return result;

            //var content = await CacheManager.GetOrCreateAsync<string>(cacheKey, async entry =>
            //{
            //    //SendAsync异常(请求、超时异常)，会throw
            //    //服务端异常，不会抛出
            //    var responseResult = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            //    if (!responseResult.IsSuccessStatusCode)
            //    {
            //        entry.AbsoluteExpirationRelativeToNow = null;
            //        return null;
            //    }

            //    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(milliseconds);
            //    return await responseResult.Content.ReadAsStringAsync();
            //});
        }
    }
}
