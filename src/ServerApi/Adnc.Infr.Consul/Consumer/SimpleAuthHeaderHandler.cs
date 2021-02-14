using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Adnc.Infr.Consul.Consumer
{
    public class SimpleAuthHeaderHandler : DelegatingHandler
    {
        private readonly Func<Task<string>> _token;
        public Guid ContextId { get; private set; }

        public SimpleAuthHeaderHandler(Func<Task<string>> token = null)
        {
            _token = token;
            this.ContextId = Guid.NewGuid();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = request.Headers;

            var auth = headers.Authorization;
            if (auth != null && _token != null)
            {
                var tokenTxt = await _token();
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

                        bool existCache = CacheManager.TryGetValue(cacheKey, out string content);
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
                            CacheManager.Set(cacheKey, await responseResult.Content.ReadAsStringAsync());

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
