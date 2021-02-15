using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Consul;

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
            var current = request.RequestUri;
            var cacheKey = $"service_consul_url_{current.Host }";
            try
            {
                var auth = request.Headers.Authorization;
                if (auth != null)
                {
                    var tokenTxt = _tokenGenerator?.Create();
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
                }

                var serverUrl = _memoryCache.GetOrCreate(cacheKey, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
                    return GetServiceAddress(current.Host);
                });

                request.RequestUri = new Uri($"{current.Scheme}://{serverUrl}{current.PathAndQuery}");
                var responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                return responseMessage;
            }
            catch (Exception ex)
            {
                _memoryCache.Remove(cacheKey);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                request.RequestUri = current;
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