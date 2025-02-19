using Adnc.Shared.WebApi.Authentication;
using Adnc.Shared.WebApi.Authentication.Bearer;
using Adnc.Shared.WebApi.Authorization;

namespace Adnc.Shared.WebApi.Registrar;

//public abstract partial class AbstractWebApiDependencyRegistrar : IDependencyRegistrar
public abstract partial class AbstractWebApiDependencyRegistrar
{
    public string Name => "webapi";
    protected IConfiguration Configuration { get; init; } = default!;
    protected IServiceCollection Services { get; init; } = default!;
    protected IServiceInfo ServiceInfo { get; init; } = default!;

    /// <summary>
    /// 服务注册与系统配置
    /// </summary>
    /// <param name="services"><see cref="IServiceInfo"/></param>
    public AbstractWebApiDependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo)
    {
        Services = services;
        Configuration = services.GetConfiguration();
        ServiceInfo = serviceInfo;
    }

    public abstract void AddAdncServices();

    /// <summary>
    /// 注册Webapi通用的服务
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public virtual void AddWebApiDefaultServices() =>
        AddWebApiDefaultServices<BearerAuthenticationCacheProcessor, PermissionCacheHandler>();

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
            .Configure<JWTOptions>(Configuration.GetSection(NodeConsts.JWT))
            .Configure<ThreadPoolSettings>(Configuration.GetSection(NodeConsts.ThreadPoolSettings))
            .Configure<KestrelOptions>(Configuration.GetSection(NodeConsts.Kestrel));

        Services
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