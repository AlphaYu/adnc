using Adnc.Infra.Consul.Consumer;
using Autofac;
using Consul;
using System;

namespace Adnc.Infra.Consul
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncInfrConsulModule : Module
    {
        private readonly string _consulAddress;

        public AdncInfrConsulModule(string consulAddress)
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
                   .InstancePerLifetimeScope();

            builder.Register(x => new ConsulClient(cfg =>
            {
                cfg.Address = new Uri(_consulAddress);
            }))
            .AsSelf()
            .InstancePerLifetimeScope();
        }
    }
}