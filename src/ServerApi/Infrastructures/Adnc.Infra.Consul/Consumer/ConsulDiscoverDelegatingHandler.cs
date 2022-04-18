using Adnc.Infra.Consul.TokenGenerator;
using Consul;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public class ConsulDiscoverDelegatingHandler : DelegatingHandler
    {
        private readonly static SemaphoreSlim _slimlock = new SemaphoreSlim(1, 1);
        private readonly ConsulClient _consulClient;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ConsulDiscoverDelegatingHandler> _logger;

        public ConsulDiscoverDelegatingHandler(ConsulClient consulClient
            , ITokenGenerator tokenGenerator
            , IMemoryCache memoryCache
            , ILogger<ConsulDiscoverDelegatingHandler> logger)
        {
            _consulClient = consulClient;
            _tokenGenerator = tokenGenerator;
            _memoryCache = memoryCache;
            _logger = logger;
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
                var serviceUrls = await GetAllHealthServiceAddressAsync(currentUri.Host, serviceAddressCacheKey);
                var serviceUrl = GetServiceAddress(serviceUrls);
                if (serviceUrl.IsNullOrWhiteSpace())
                    throw new ArgumentNullException($"{currentUri.Host} does not contain helath service address!");
                else
                    request.RequestUri = new Uri($"{currentUri.Scheme}://{serviceUrl}{currentUri.PathAndQuery}");

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

                #endregion 缓存处理

                var responseMessage = await base.SendAsync(request, cancellationToken);
                return responseMessage;
            }
            catch (Exception)
            {
                _memoryCache.Remove(serviceAddressCacheKey);
                throw;
            }
            finally
            {
                request.RequestUri = currentUri;
            }
        }

        private string GetServiceAddress(IEnumerable<string> healthAddresses)
        {
            if (healthAddresses != null && healthAddresses.Any())
            {
                int index = new Random().Next(healthAddresses.Count());
                var address = healthAddresses.ElementAt(index);
                return address;
            }
            return default;
        }

        private async Task<List<string>> GetAllHealthServiceAddressAsync(string serviceName, string serviceAddressCacheKey)
        {
            var healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
            if (healthAddresses != null && healthAddresses.Any())
            {
                return healthAddresses;
            }

            await _slimlock.WaitAsync();

            try
            {
                _logger.LogInformation($"SemaphoreSlim=true,{serviceAddressCacheKey}");
                healthAddresses = _memoryCache.Get<List<string>>(serviceAddressCacheKey);
                if (healthAddresses != null && healthAddresses.Any())
                {
                    return healthAddresses;
                }
                var query = await _consulClient.Health.Service(serviceName, string.Empty, true);
                var servicesEntries = query.Response;
                if (servicesEntries != null && servicesEntries.Any())
                {
                    var entryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
                    };
                    healthAddresses = servicesEntries.Select(entry => $"{entry.Service.Address}:{entry.Service.Port}").ToList();
                    _memoryCache.Set(serviceAddressCacheKey, healthAddresses, entryOptions);
                }
                return healthAddresses;
            }
            finally
            {
                _slimlock.Release();
            }
        }
    }
}