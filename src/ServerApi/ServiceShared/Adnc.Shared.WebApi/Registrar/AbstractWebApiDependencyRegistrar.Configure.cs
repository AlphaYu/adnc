namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册配置类到IOC容器
    /// </summary>
    protected virtual void Configure()
    {
        Services
            .Configure<JWTOptions>(Configuration.GetSection(NodeConsts.JWT))
            .Configure<ThreadPoolSettings>(Configuration.GetSection(NodeConsts.ThreadPoolSettings))
            .Configure<KestrelOptions>(Configuration.GetSection(NodeConsts.Kestrel))
            ;
    }
}
