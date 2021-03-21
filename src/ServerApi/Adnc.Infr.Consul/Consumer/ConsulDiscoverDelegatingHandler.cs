using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Consul;
using System.Text;

namespace Adnc.Infr.Consul.Consumer
{
    public class ConsulDiscoverDelegatingHandler : DelegatingHandler
    {
        private readonly ConsulClient _consulClient;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMemoryCache _memoryCache;

        public ConsulDiscoverDelegatingHandler(ConsulClient consulClient
            , ITokenGenerator tokenGenerator
            , IMemoryCache memoryCache)
        {
            _consulClient = consulClient;
            _tokenGenerator = tokenGenerator;
            _memoryCache = memoryCache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request
            , CancellationToken cancellationToken)
        {
            var headers = request.Headers;
            var currentUri = request.RequestUri;
            var serviceAddressCacheKey = $"service_consul_url_{currentUri.Host }";

            try
            {
                var auth = headers.Authorization;
                if (auth != null)
                {
                    var tokenTxt = _tokenGenerator?.Create();

                    if (!string.IsNullOrEmpty(tokenTxt))
                        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
                }

                var serverUrl = _memoryCache.GetOrCreate(serviceAddressCacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                    return GetServiceAddress(currentUri.Host);
                });

                request.RequestUri = new Uri($"{currentUri.Scheme}://{serverUrl}{currentUri.PathAndQuery}");

                //如果调用地址是https,使用http2
                if (request.RequestUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                    request.Version = new Version(2, 0);

                #region 缓存处理
                /* 这里高并发会有问题，需要优化，先注释
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
                                _memoryCache.Set(cacheKey, await responseResult.Content.ReadAsStringAsync(), TimeSpan.FromMilliseconds(milliseconds));

                            return responseResult;
                        }
                    }
                }
                */
                #endregion

                var responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                return responseMessage;
            }
            catch (Exception ex)
            {
                _memoryCache.Remove(serviceAddressCacheKey);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                request.RequestUri = currentUri;
            }
        }

        private string GetServiceAddress(string serviceName)
        {
            var servicesEntry = _consulClient.Health.Service(serviceName, string.Empty, true).Result.Response;
            if (servicesEntry != null && servicesEntry.Any())
            {
                int index = new Random().Next(servicesEntry.Count());
                var entry = servicesEntry.ElementAt(index);
                return $"{entry.Service.Address}:{entry.Service.Port}";
            }
            return null;
        }
    }
}