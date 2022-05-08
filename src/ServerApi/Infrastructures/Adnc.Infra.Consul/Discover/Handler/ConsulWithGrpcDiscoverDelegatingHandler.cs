using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace Adnc.Infra.Consul.Discover;

public class ConsulWithGrpcDiscoverDelegatingHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim _slimlock = new(1, 1);
    private readonly ConsulClient _consulClient;
    private readonly IEnumerable<ITokenGenerator> _tokenGenerators;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ConsulWithGrpcDiscoverDelegatingHandler> _logger;
    private readonly KestrelConfig _kestrelConfig;

    public ConsulWithGrpcDiscoverDelegatingHandler(ConsulClient consulClient
        , IEnumerable<ITokenGenerator> tokenGenerators
        , IMemoryCache memoryCache
        , ILogger<ConsulWithGrpcDiscoverDelegatingHandler> logger
        , IOptions<KestrelConfig> kestrelOptions
        )
    {
        _consulClient = consulClient;
        _tokenGenerators = tokenGenerators;
        _memoryCache = memoryCache;
        _logger = logger;
        _kestrelConfig = kestrelOptions?.Value;
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
                var tokenGenerator = _tokenGenerators.FirstOrDefault(x => x.Scheme.EqualsIgnoreCase(auth.Scheme));
                var tokenTxt = tokenGenerator?.Create();

                if (!string.IsNullOrEmpty(tokenTxt))
                    request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, tokenTxt);
            }
            var serviceUrls = await GetAllHealthServiceAddressAsync(currentUri.Host, serviceAddressCacheKey);
            var serviceUrl = LoadRandomBalancer(serviceUrls);
            if (serviceUrl.IsNullOrWhiteSpace())
                throw new ArgumentNullException($"{currentUri.Host} does not contain helath service address!");
            else
            {
                var serviceUriArray = serviceUrl.Split(':');
                var scheme = currentUri.Scheme;
                var host = serviceUriArray[0];
                var port = serviceUriArray[1];
                var pathAndQuery = currentUri.PathAndQuery;
                if (_kestrelConfig is not null)
                {
                    var defaultEnpoint = _kestrelConfig.Endpoints.FirstOrDefault(x => x.Key.EqualsIgnoreCase("default")).Value;
                    if (defaultEnpoint is not null || defaultEnpoint.Url.IsNotNullOrWhiteSpace())
                        port = new Uri(defaultEnpoint.Url).Port.ToString();
                }
                request.RequestUri = new Uri($"{scheme}://{host}:{port}/{pathAndQuery}");
            }

            if (request.RequestUri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase) && request.Version != new Version(2, 0))
                request.Version = new Version(2, 0);

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

    private string LoadRandomBalancer(IEnumerable<string> healthAddresses)
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