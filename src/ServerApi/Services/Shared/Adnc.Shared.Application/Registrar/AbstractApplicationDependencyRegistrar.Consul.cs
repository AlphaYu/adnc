namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
{
    /// <summary>
    /// 注册Consul相关服务
    /// </summary>
    /// <param name="action"></param>
    protected virtual void AddConsulServices(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        if (ConsulSection is not null)
            Services.AddAdncInfraConsul(ConsulSection);
    }
}