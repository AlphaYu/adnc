using Polly.Timeout;

namespace Adnc.Shared.Application.Registrar
{
    public static class AbstractApplicationDependencyRegistrarExtension
    {
        /// <summary>
        /// default rest policies
        /// </summary>
        /// <returns></returns>
        public static List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultRefitPolicies(this AbstractApplicationDependencyRegistrar _)
        {
            //隔离策略
            //var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(10, 100);

            //回退策略
            //回退也称服务降级，用来指定发生故障时的备用方案。
            //目前用不上
            //var fallbackPolicy = Policy<string>.Handle<HttpRequestException>().FallbackAsync("substitute data");

            //缓存策略
            //缓存策略无效
            //https://github.com/App-vNext/Polly/wiki/Polly-and-HttpClientFactory?WT.mc_id=-blog-scottha#user-content-use-case-cachep
            //var cache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            //var cacheProvider = new MemoryCacheProvider(cache);
            //var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(cacheProvider, TimeSpan.FromSeconds(100));

            //重试策略,超时或者API返回>500的错误,重试3次。
            //重试次数会统计到失败次数
            var retryPolicy = Policy.Handle<TimeoutRejectedException>()
                                    .OrResult<HttpResponseMessage>(response => (int)response.StatusCode >= 500)
                                    .WaitAndRetryAsync(new[]
                                    {
                                    TimeSpan.FromSeconds(3),
                                    TimeSpan.FromSeconds(5),
                                    });
            //超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(IsDevelopment(_) ? 10 : 9);

            //熔断策略
            //如下，如果我们的业务代码连续失败50次，就触发熔断(onBreak),就不会再调用我们的业务代码，而是直接抛出BrokenCircuitException异常。
            //当熔断时间10分钟后(durationOfBreak)，切换为HalfOpen状态，触发onHalfOpen事件，此时会再调用一次我们的业务代码
            //，如果调用成功，则触发onReset事件，并解除熔断，恢复初始状态，否则立即切回熔断状态。
            var circuitBreakerPolicy = Policy.Handle<Exception>()
                                             .CircuitBreakerAsync
                                             (
                                                 // 熔断前允许出现几次错误
                                                 exceptionsAllowedBeforeBreaking: 10
                                                 ,
                                                 // 熔断时间,熔断10分钟
                                                 durationOfBreak: TimeSpan.FromMinutes(3)
                                                 ,
                                                 // 熔断时触发
                                                 onBreak: (ex, breakDelay) =>
                                                 {
                                                     //todo
                                                     var e = ex;
                                                     var delay = breakDelay;
                                                 }
                                                 ,
                                                 //熔断恢复时触发
                                                 onReset: () =>
                                                 {
                                                     //todo
                                                 }
                                                 ,
                                                 //在熔断时间到了之后触发
                                                 onHalfOpen: () =>
                                                 {
                                                     //todo
                                                 }
                                             );

            return new List<IAsyncPolicy<HttpResponseMessage>>()
                        {
                            retryPolicy
                           ,timeoutPolicy
                           ,circuitBreakerPolicy.AsAsyncPolicy<HttpResponseMessage>()
                        };
        }

        /// <summary>
        /// default grpc policies
        /// </summary>
        /// <param name="registrar"></param>
        /// <returns></returns>
        public static List<IAsyncPolicy<HttpResponseMessage>> GenerateDefaultGrpcPolicies(this AbstractApplicationDependencyRegistrar registrar) => 
            GenerateDefaultRefitPolicies(registrar);


        public static string ASPNETCORE_ENVIRONMENT => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static bool IsDevelopment(this AbstractApplicationDependencyRegistrar _) => ASPNETCORE_ENVIRONMENT.EqualsIgnoreCase("Development");

        public static string GetEnvShortName(this AbstractApplicationDependencyRegistrar _)
        {
            return ASPNETCORE_ENVIRONMENT.ToLower() switch
            {
                "development" => "dev",
                "test" => "test",
                "staging" => $"stag",
                "production" => $"prod",
                _ => throw new NullReferenceException(nameof(ASPNETCORE_ENVIRONMENT))
            };
        }

        public static bool IsEnableSkyApm(this AbstractApplicationDependencyRegistrar _)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_HOSTINGSTARTUPASSEMBLIES");
            if (env.IsNullOrEmpty())
                return false;
            else
                return env.Contains("SkyAPM.Agent.AspNetCore");
        }
    }
}