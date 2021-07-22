using Adnc.Infra.Consul.TokenGenerator;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public class SimpleDiscoveryDelegatingHandler : DelegatingHandler
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMemoryCache _memoryCache;

        public SimpleDiscoveryDelegatingHandler(ITokenGenerator tokenGenerator
            , IMemoryCache memoryCache)
        {
            _tokenGenerator = tokenGenerator;
            _memoryCache = memoryCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //如果调用地址是https,使用http2
            if (request.RequestUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                request.Version = new Version(2, 0);

            var headers = request.Headers;

            var auth = headers.Authorization;
            if (auth != null)
            {
                var tokenTxt = _tokenGenerator?.Create();

                if (!string.IsNullOrEmpty(tokenTxt))
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }

            #region 缓存处理

            //if (request.Method == HttpMethod.Get)
            //{
            //    var cache = headers.FirstOrDefault(x => x.Key == "Cache");

            //    if (!string.IsNullOrWhiteSpace(cache.Key))
            //    {
            //        int.TryParse(cache.Value.FirstOrDefault(), out int milliseconds);

            //        if (milliseconds > 0)
            //        {
            //            var cacheKey = request.RequestUri.AbsoluteUri.GetHashCode();

            //            var existCache = _memoryCache.TryGetValue(cacheKey, out string content);
            //            if (existCache)
            //            {
            //                var resp = new HttpResponseMessage
            //                {
            //                    Content = new StringContent(content, Encoding.UTF8)
            //                };

            //                return resp.EnsureSuccessStatusCode();
            //            }

            //            //SendAsync异常(请求、超时异常)，会throw
            //            //服务端异常，不会抛出
            //            var responseResult = await base.SendAsync(request, cancellationToken).ConfigureAwait(true);
            //            if (responseResult.IsSuccessStatusCode)
            //                _memoryCache.Set(cacheKey, await responseResult.Content.ReadAsStringAsync(), TimeSpan.FromMilliseconds(milliseconds));

            //            return responseResult;
            //        }
            //    }
            //}

            #endregion 缓存处理

            return await base.SendAsync(request, cancellationToken);
        }
    }
}