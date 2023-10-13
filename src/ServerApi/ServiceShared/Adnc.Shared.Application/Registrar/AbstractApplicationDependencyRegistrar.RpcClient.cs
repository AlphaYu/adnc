using Adnc.Infra.Consul.Discover.GrpcResolver;
using Adnc.Infra.Consul.Discover.Handler;
using Adnc.Shared.Rpc.Handlers;
using Adnc.Shared.Rpc.Handlers.Token;
using Grpc.Core;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    private static bool _theFirstCalled = true; 

    /// <summary>
    /// 注册Rest服务(跨微服务之间的同步通讯)
    /// </summary>
    /// <typeparam name="TRestClient">Rpc服务接口</typeparam>
    /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
    /// <param name="policies">Polly策略</param>
    protected virtual void AddRestClient<TRestClient>(string serviceName, List<IAsyncPolicy<HttpResponseMessage>> policies)
     where TRestClient : class
    {
        var addressNode = RpcAddressInfo.FirstOrDefault(x => x.Service.EqualsIgnoreCase(serviceName));
        if (addressNode is null)
            throw new NullReferenceException(nameof(addressNode));

        if(_theFirstCalled)
        {
            _theFirstCalled = false;
            Services.AddScoped<CacheDelegatingHandler>();
            Services.AddScoped<TokenDelegatingHandler>();
            Services.AddScoped<ConsulDiscoverDelegatingHandler>();
            Services.AddSingleton<TokenFactory>();
            Services.AddSingleton<ITokenGenerator, BasicTokenGenerator>();
            Services.AddSingleton<ITokenGenerator, BearerTokenGenerator>();
        }

        var registeredType = Configuration.GetValue(NodeConsts.RegisteredType, "direct");
        //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
        var contentSerializer = new SystemTextJsonContentSerializer(SystemTextJson.GetAdncDefaultOptions());
        var refitSettings = new RefitSettings(contentSerializer);
        var clientbuilder = Services.AddRefitClient<TRestClient>(refitSettings)
                                                    .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                                                    .AddPolicyHandlerICollection(policies)
                                                    //.UseHttpClientMetrics()
                                                    .AddHttpMessageHandler<CacheDelegatingHandler>()
                                                    .AddHttpMessageHandler<TokenDelegatingHandler>();
        switch (registeredType)
        {
            case RegisteredTypeConsts.Direct:
                {
                    clientbuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.Direct));
                    break;
                }
            case RegisteredTypeConsts.ClusterIP:
                {
                    clientbuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.CoreDns));
                    break;
                }
            case RegisteredTypeConsts.Consul:
                {
                    clientbuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.Consul))
                                        .AddHttpMessageHandler<ConsulDiscoverDelegatingHandler>();
                    break;
                }
            default: 
                throw new NotImplementedException(registeredType);
        }
    }


    /// <summary>
    /// 注册Grpc服务(跨微服务之间的同步通讯)
    /// </summary>
    /// <typeparam name="TRpcService">Rpc服务接口</typeparam>
    /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
    /// <param name="policies">Polly策略</param>
    protected virtual void AddGrpcClient<TGrpcClient>(string serviceName, List<IAsyncPolicy<HttpResponseMessage>> policies)
     where TGrpcClient : class
    {
        var addressNode = RpcAddressInfo.FirstOrDefault(x => x.Service.EqualsIgnoreCase(serviceName));
        if (addressNode is null)
            throw new NullReferenceException(nameof(addressNode));

        if(_theFirstCalled)
        {
            _theFirstCalled = false;
            Services.AddScoped<CacheDelegatingHandler>();
            Services.AddScoped<TokenDelegatingHandler>();
            Services.AddScoped<ConsulDiscoverDelegatingHandler>();
            Services.AddSingleton<TokenFactory>();
            Services.AddSingleton<ITokenGenerator, BasicTokenGenerator>();
            Services.AddSingleton<ITokenGenerator, BearerTokenGenerator>();
        }

        var registeredType = Configuration.GetValue(NodeConsts.RegisteredType, "direct");
        var switchName = "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport";
        var switchResult = AppContext.TryGetSwitch(switchName, out bool isEnabled);
        if (!switchResult || !isEnabled)
            AppContext.SetSwitch(switchName, true);

        var baseAddress = string.Empty;
        switch (registeredType)
        {
            case RegisteredTypeConsts.Direct:
                {
                    var restBaseAddress = new Uri(addressNode.Direct);
                    baseAddress = $"{restBaseAddress.Scheme}://{restBaseAddress.Host}:{restBaseAddress.Port + 1}";
                    break;
                }
            case RegisteredTypeConsts.ClusterIP:
                {
                    baseAddress = addressNode.CoreDns.Replace("http://", "dns://").Replace("https://", "dns://");
                    break;
                }
            case RegisteredTypeConsts.Consul:
                {
                    baseAddress = addressNode.Consul.Replace("http://", "consul://").Replace("https://", "consul://");
                    Services.TryAddSingleton<ResolverFactory, ConsulGrpcResolverFactory>();
                    break;
                }
        }

        Services.AddGrpcClient<TGrpcClient>(options => options.Address = new Uri(baseAddress))
                     .ConfigureChannel(options =>
                     {
                         options.Credentials = ChannelCredentials.Insecure;
                         options.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
                     })
                     .AddHttpMessageHandler<TokenDelegatingHandler>()
                     .AddPolicyHandlerICollection(policies);
    }

}

public class AddressNode
{
    public string Service { get; set; } = string.Empty;
    public string Direct { get; set; } = string.Empty;
    public string Consul { get; set; } = string.Empty;
    public string CoreDns { get; set; } = string.Empty;
}