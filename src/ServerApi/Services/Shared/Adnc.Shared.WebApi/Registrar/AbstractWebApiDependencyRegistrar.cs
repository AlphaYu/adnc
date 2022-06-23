using Adnc.Shared.WebApi.Authentication;

namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar : IDependencyRegistrar
{
    public string Name => "webapi";
    protected IConfiguration Configuration;
    protected readonly IServiceCollection Services;
    protected readonly IHostEnvironment HostEnvironment;
    protected readonly IServiceInfo ServiceInfo;

    /// <summary>
    /// 服务注册与系统配置
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="services"><see cref="IServiceInfo"/></param>
    /// <param name="environment"><see cref="IHostEnvironment"/></param>
    /// <param name="serviceInfo"><see cref="ServiceInfo"/></param>
    protected AbstractWebApiDependencyRegistrar(IServiceCollection services)
    {
        Services = services;
        Configuration = services.GetConfiguration();
        ServiceInfo = services.GetServiceInfo();
    }

    /// <summary>
    /// 注册服务入口方法
    /// </summary>
    public abstract void AddAdnc();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    protected virtual void AddWebApiDefault() =>
        AddWebApiDefault<BearerAuthenticationRemoteProcessor, PermissionRemoteHandler>();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    protected virtual void AddWebApiDefault<TAuthenticationHandler, TAuthorizationHandler>()
        where TAuthenticationHandler : AbstracAuthenticationProcessor
        where TAuthorizationHandler : AbstractPermissionHandler
    {
        Services
            .AddHttpContextAccessor()
            .AddMemoryCache();
        Configure();
        AddControllers();
        AddAuthentication<TAuthenticationHandler>();
        AddAuthorization<TAuthorizationHandler>();
        AddCors();
        AddSwaggerGen();
        AddHealthChecks();
        AddMiniProfiler();
        AddApplicationServices();
    }
}