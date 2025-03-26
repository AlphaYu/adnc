using Adnc.Infra.Consul.Discover.GrpcResolver;
using Adnc.Infra.Consul.Discover.Handler;
using Adnc.Shared.Remote;
using Adnc.Shared.Remote.Handlers;
using Adnc.Shared.Remote.Handlers.Token;
using Adnc.Shared.Remote.Http;
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
     where TRestClient : class,IRestClient
    {
        if(string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentNullException(nameof(serviceName));
        }

        if (RpcInfoOption is null)
        {
            throw new NullReferenceException(nameof(RpcInfoOption));
        }
        else
        {
            AddRpcClientCommonServices(Services, RpcInfoOption);
        }

        var enablePolly = RpcInfoOption.Polly.Enable;
        //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
        var contentSerializer = new SystemTextJsonContentSerializer(SystemTextJson.GetAdncDefaultOptions());
        var refitSettings = new RefitSettings(contentSerializer);
        var clientbuilder = Services.AddRefitClient<TRestClient>(refitSettings)
                                                    .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                                                    .AddPolicyHandlerICollection(enablePolly ? policies : [])
                                                    //.UseHttpClientMetrics()
                                                    .AddHttpMessageHandler<CacheDelegatingHandler>()
                                                    .AddHttpMessageHandler<TokenDelegatingHandler>();

        var addressNode = RpcInfoOption.Address.First(x => x.Service.EqualsIgnoreCase(serviceName));
        switch (RegisterType)
        {
            case RegisteredTypeConsts.Direct:
                {
                    clientbuilder.ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(addressNode.Direct));
                    break;
                }
            case RegisteredTypeConsts.CoreDns:
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
            case RegisteredTypeConsts.Nacos:
                {
                    //todo
                    break;
                }
            default: 
                throw new NotImplementedException(RegisterType);
        }
    }

    /// <summary>
    /// 注册Grpc服务(跨微服务之间的同步通讯)
    /// </summary>
    /// <typeparam name="TRpcService">Rpc服务接口</typeparam>
    /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
    /// <param name="policies">Polly策略</param>
    protected virtual void AddGrpcClient<TGrpcClient>(string serviceName, List<IAsyncPolicy<HttpResponseMessage>> policies)
     where TGrpcClient : ClientBase<TGrpcClient>
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentNullException(nameof(serviceName));
        }

        if (RpcInfoOption is null)
        {
            throw new NullReferenceException(nameof(RpcInfoOption));
        }
        else
        {
            AddRpcClientCommonServices(Services, RpcInfoOption);
        }

        var switchName = "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport";
        var switchResult = AppContext.TryGetSwitch(switchName, out bool isEnabled);
        if (!switchResult || !isEnabled)
        {
            AppContext.SetSwitch(switchName, true);
        }

        var baseAddress = string.Empty;
        var addressNode = RpcInfoOption.Address.First(x => x.Service.EqualsIgnoreCase(serviceName));
        switch (RegisterType)
        {
            case RegisteredTypeConsts.Direct:
                {
                    var restBaseAddress = new Uri(addressNode.Direct);
                    baseAddress = $"{restBaseAddress.Scheme}://{restBaseAddress.Host}:{restBaseAddress.Port + 1}";
                    break;
                }
            case RegisteredTypeConsts.CoreDns:
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
            case RegisteredTypeConsts.Nacos:
                {
                    //todo
                    break;
                }
            default:
                throw new NotImplementedException(RegisterType);
        }

        var enablePolly = RpcInfoOption.Polly.Enable;
        Services.AddGrpcClient<TGrpcClient>(options => options.Address = new Uri(baseAddress))
                     .ConfigureChannel(options =>
                     {
                         options.Credentials = ChannelCredentials.Insecure;
                         options.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
                     })
                     .AddHttpMessageHandler<TokenDelegatingHandler>()
                     .AddPolicyHandlerICollection(enablePolly ? policies : []);
    }

    /// <summary>
    /// 注册RpcClient通用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rpcInfo"></param>
    private static void AddRpcClientCommonServices(IServiceCollection services, RpcInfo rpcInfo)
    {
        if (_theFirstCalled)
        {
            _theFirstCalled = false;
            services.AddSingleton(rpcInfo);
            services.AddScoped<CacheDelegatingHandler>();
            services.AddScoped<TokenDelegatingHandler>();
            services.AddScoped<ConsulDiscoverDelegatingHandler>();
            services.AddSingleton<TokenFactory>();
            services.AddSingleton<ITokenGenerator, BasicTokenGenerator>();
            services.AddSingleton<ITokenGenerator, BearerTokenGenerator>();
        }
    }
}