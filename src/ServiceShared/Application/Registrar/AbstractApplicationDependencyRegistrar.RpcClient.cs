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
     where TRestClient : class, IRestClient
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(serviceName, nameof(serviceName));
        ArgumentNullException.ThrowIfNull(policies, nameof(policies));

        var registerType = Configuration.GetValue<string>(NodeConsts.RegisterType) ?? RegisteredTypeConsts.Direct;
        var rpcInfo = Configuration.GetRequiredSection(NodeConsts.RpcInfo).Get<RpcInfo>() ?? throw new InvalidDataException(nameof(NodeConsts.RpcInfo));
        AddRpcClientCommonServices(Services, rpcInfo);

        var enablePolly = rpcInfo.Polly.Enable;
        //注册RefitClient,设置httpclient生命周期时间，默认也是2分钟。
        var contentSerializer = new SystemTextJsonContentSerializer(SystemTextJson.GetAdncDefaultOptions());
        var refitSettings = new RefitSettings(contentSerializer);
        var clientbuilder = Services.AddRefitClient<TRestClient>(refitSettings)
                                                    .SetHandlerLifetime(TimeSpan.FromMinutes(2))
                                                    .AddPolicyHandlers(enablePolly ? policies : [])
                                                    //.UseHttpClientMetrics()
                                                    .AddHttpMessageHandler<CacheDelegatingHandler>()
                                                    .AddHttpMessageHandler<TokenDelegatingHandler>();

        var addressNode = rpcInfo.Address.First(x => x.Service.EqualsIgnoreCase(serviceName));
        switch (registerType)
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
                throw new NotImplementedException(registerType);
        }
    }

    /// <summary>
    /// 注册Grpc服务(跨微服务之间的同步通讯)
    /// </summary>
    /// <typeparam name="TGrpcClient"></typeparam>
    /// <param name="serviceName">在注册中心注册的服务名称，或者服务的Url</param>
    /// <param name="policies"></param>
    /// <exception cref="InvalidDataException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void AddGrpcClient<TGrpcClient>(string serviceName, List<IAsyncPolicy<HttpResponseMessage>> policies)
     where TGrpcClient : ClientBase<TGrpcClient>
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(serviceName, nameof(serviceName));
        ArgumentNullException.ThrowIfNull(policies, nameof(policies));

        var registerType = Configuration.GetValue<string>(NodeConsts.RegisterType) ?? RegisteredTypeConsts.Direct;
        var rpcInfo = Configuration.GetRequiredSection(NodeConsts.RpcInfo).Get<RpcInfo>() ?? throw new InvalidDataException(nameof(NodeConsts.RpcInfo));
        AddRpcClientCommonServices(Services, rpcInfo);

        var switchName = "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport";
        var switchResult = AppContext.TryGetSwitch(switchName, out var isEnabled);
        if (!switchResult || !isEnabled)
        {
            AppContext.SetSwitch(switchName, true);
        }

        var baseAddress = string.Empty;
        var addressNode = rpcInfo.Address.First(x => x.Service.EqualsIgnoreCase(serviceName));
        switch (registerType)
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
                throw new NotImplementedException(registerType);
        }

        var enablePolly = rpcInfo.Polly.Enable;
        Services.AddGrpcClient<TGrpcClient>(options => options.Address = new Uri(baseAddress))
                     .ConfigureChannel(options =>
                     {
                         options.Credentials = ChannelCredentials.Insecure;
                         options.ServiceConfig = new ServiceConfig { LoadBalancingConfigs = { new RoundRobinConfig() } };
                         options.UnsafeUseInsecureChannelCallCredentials = true;
                     })
                     .AddHttpMessageHandler<TokenDelegatingHandler>()
                     .AddPolicyHandlers(enablePolly ? policies : []);
    }

    /// <summary>
    /// 注册RpcClient通用服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="rpcInfo"></param>
    private void AddRpcClientCommonServices(IServiceCollection services, RpcInfo rpcInfo)
    {
        if (_theFirstCalled)
        {
            _theFirstCalled = false;
            services.AddSingleton(rpcInfo);
            services.Add(new ServiceDescriptor(typeof(CacheDelegatingHandler), typeof(CacheDelegatingHandler), Lifetime));
            services.Add(new ServiceDescriptor(typeof(TokenDelegatingHandler), typeof(TokenDelegatingHandler), Lifetime));
            services.Add(new ServiceDescriptor(typeof(ConsulDiscoverDelegatingHandler), typeof(ConsulDiscoverDelegatingHandler), Lifetime));
            services.AddSingleton<TokenFactory>();
            services.AddSingleton<ITokenGenerator, BasicTokenGenerator>();
            services.AddSingleton<ITokenGenerator, BearerTokenGenerator>();
        }
    }
}
