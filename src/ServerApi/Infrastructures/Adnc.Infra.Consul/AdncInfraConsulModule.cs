namespace Adnc.Infra.Consul;

/// <summary>
/// Autofac注册
/// </summary>
public class AdncInfraConsulModule : Autofac.Module
{
    private readonly string _consulAddress;

    public AdncInfraConsulModule(string consulAddress)
    {
        _consulAddress = consulAddress;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<DefaultTokenGenerator>()
               .As<ITokenGenerator>()
               .InstancePerLifetimeScope();

        builder.RegisterType<SimpleDiscoveryDelegatingHandler>()
               .AsSelf()
               .InstancePerLifetimeScope();

        builder.RegisterType<ConsulDiscoverDelegatingHandler>()
               .AsSelf()
               //.WithParameter("consulAddress", _consulAddress)
               .InstancePerLifetimeScope();

        builder.Register(x => new ConsulClient(cfg =>
        {
            cfg.Address = new Uri(_consulAddress);
        }))
        .AsSelf()
        .SingleInstance();
    }
}