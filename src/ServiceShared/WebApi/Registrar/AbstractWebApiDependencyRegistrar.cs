using Adnc.Shared.WebApi.Authentication;
using Adnc.Shared.WebApi.Authorization;

namespace Adnc.Shared.WebApi.Registrar;

/// <summary>
/// 服务注册与系统配置
/// </summary>
/// <param name="services"><see cref="IServiceInfo"/></param>
public abstract partial class AbstractWebApiDependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo)
{
    public string Name => "webapi";
    protected IConfiguration Configuration { get; init; } = services.GetConfiguration();
    protected IServiceCollection Services { get; init; } = services;
    protected IServiceInfo ServiceInfo { get; init; } = serviceInfo;

    public abstract void AddAdncServices();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public virtual void AddWebApiDefaultServices()
        => AddWebApiDefaultServices<BearerAuthenticationCacheProcessor, PermissionCacheHandler>();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="TAuthenticationProcessor"><see cref="AbstractAuthenticationProcessor"/></typeparam>
    /// <typeparam name="TAuthorizationHandler"><see cref="AbstractPermissionHandler"/></typeparam>
    public virtual void AddWebApiDefaultServices<TAuthenticationProcessor, TAuthorizationHandler>()
        where TAuthenticationProcessor : AbstractAuthenticationProcessor
        where TAuthorizationHandler : AbstractPermissionHandler
    {
        Services
            .Configure<ThreadPoolSettings>(Configuration.GetSection(NodeConsts.ThreadPoolSettings))
            .AddHttpContextAccessor()
            .AddMemoryCache();

        AddControllers();
        AddAuthentication<TAuthenticationProcessor>();
        AddAuthorization<TAuthorizationHandler>();
        AddCors();

        var enableSwaggerUI = Configuration.GetValue(NodeConsts.SwaggerUI_Enable, true);
        if(enableSwaggerUI)
        {
            AddSwaggerGen();
            AddMiniProfiler();
        }
    }
}