using Adnc.Infra.Consul.Consumer;
using Adnc.Infra.Consul.TokenGenerator;
using Autofac;
using Consul;
using System;

namespace Adnc.Infra.Consul
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncInfraConsulModule : Module
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
                   .SingleInstance();

            builder.RegisterType<SimpleDiscoveryDelegatingHandler>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ConsulDiscoverDelegatingHandler>()
                   .AsSelf()
                   .WithParameter("consulAddress", _consulAddress)
                   .SingleInstance();

            builder.Register(x => new ConsulClient(cfg =>
            {
                cfg.Address = new Uri(_consulAddress);
            }))
            .AsSelf()
            .SingleInstance();
        }
    }
}