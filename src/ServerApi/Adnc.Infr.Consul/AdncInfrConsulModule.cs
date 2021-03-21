using Adnc.Infr.Consul.Consumer;
using Autofac;
using Consul;
using Microsoft.Extensions.Options;
using System;

namespace Adnc.Infr.Consul
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncInfrConsulModule : Module
    {
        private readonly ConsulConfig _config;
        public AdncInfrConsulModule(IOptions<ConsulConfig> options)
        {
            _config = options.Value;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注册依赖模块
            this.LoadDepends(builder);

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
                cfg.Address = new Uri(_config.ConsulUrl);
            }))
            .AsSelf()
            .InstancePerLifetimeScope();
        }

        private void LoadDepends(ContainerBuilder builder)
        {
        }
    }
}