using Adnc.Shared.Application.Registrar;
using Polly.Timeout;

namespace Adnc.Shared.Application.Extensions;

public static class DependencyRegistrarExtension
{
    public static string ASPNETCORE_ENVIRONMENT => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT is null");

    /// <summary>
    /// default rest policies
    /// </summary>
    /// <returns></returns>
    public static List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultRefitPolicies(this AbstractApplicationDependencyRegistrar _)
    {
        // Bulkhead policy
        //var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);

        // Fallback policy
        // Fallback is also known as service degradation and defines a backup plan when a failure occurs.
        // Not needed for now
        //var fallbackPolicy = Policy<string>.Handle<HttpRequestException>().FallbackAsync("substitute data");

        // Cache policy
        // The cache policy is ineffective
        //https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory?WT.mc_id=-blog-scottha#user-content-use-case-cachep
        //var cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        //var cacheProvider = new MemoryCacheProvider(cache);
        //var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(cacheProvider, TimeSpan.FromSeconds(100));

        // Retry policy: retry 3 times for timeouts or API responses with status codes greater than 500.
        // Retry attempts are counted as failures
        var retryPolicy = Policy.Handle<TimeoutRejectedException>()
                                .OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500)
                                .WaitAndRetryAsync(
                                [
                                TimeSpan.FromSeconds(3),
                                TimeSpan.FromSeconds(5),
                                ]);
        // Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(_.IsDevelopment() ? 10 : 9);

        // Circuit breaker policy
        // If the business code fails continuously, the circuit breaker is triggered (onBreak).
        // The protected code will no longer be called and a BrokenCircuitException will be thrown directly.
        // After the break duration elapses, the state switches to HalfOpen and triggers onHalfOpen.
        // If the next call succeeds, onReset is triggered and the circuit is restored; otherwise it breaks again immediately.
        var circuitBreakerPolicy = Policy.Handle<Exception>()
                                         .CircuitBreakerAsync
                                         (
                                             // Number of errors allowed before breaking
                                             exceptionsAllowedBeforeBreaking: 10
                                             ,
                                             // Break duration
                                             durationOfBreak: TimeSpan.FromMinutes(3)
                                             ,
                                             // Triggered when the circuit breaks
                                             onBreak: (ex, breakDelay) =>
                                             {
                                                 //todo
                                                 var e = ex;
                                                 var delay = breakDelay;
                                             }
                                             ,
                                             // Triggered when the circuit resets
                                             onReset: () =>
                                             {
                                                 //todo
                                             }
                                             ,
                                             // Triggered when the break duration has elapsed
                                             onHalfOpen: () =>
                                             {
                                                 //todo
                                             }
                                         );

        return
                    [
                        retryPolicy
                       ,timeoutPolicy
                       ,circuitBreakerPolicy.AsAsyncPolicy<HttpResponseMessage>()
                    ];
    }

    /// <summary>
    /// default grpc policies
    /// </summary>
    /// <param name="registrar"></param>
    /// <returns></returns>
    public static List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultGrpcPolicies(this AbstractApplicationDependencyRegistrar registrar) =>
        registrar.GenerateDefaultRefitPolicies();

    public static bool IsDevelopment(this AbstractApplicationDependencyRegistrar _) => ASPNETCORE_ENVIRONMENT.EqualsIgnoreCase("Development");

    public static string GetEnvShortName(this AbstractApplicationDependencyRegistrar _)
    {
        return ASPNETCORE_ENVIRONMENT.ToLower() switch
        {
            "development" => "dev",
            "test" => "test",
            "staging" => $"stag",
            "production" => $"prod",
            _ => throw new InvalidDataException(nameof(ASPNETCORE_ENVIRONMENT))
        };
    }
}
