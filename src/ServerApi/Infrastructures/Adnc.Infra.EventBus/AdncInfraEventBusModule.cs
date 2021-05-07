using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Adnc.Infra.EventBus.RabbitMq;

namespace Adnc.Infra.Mq
{
    public class AdncInfraEventBusModule : Autofac.Module
    {
        private readonly IEnumerable<Assembly> _assembliesToScan;
        public AdncInfraEventBusModule(IEnumerable<Assembly> assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        public AdncInfraEventBusModule(params Assembly[] assembliesToScan) : this((IEnumerable<Assembly>)assembliesToScan) { }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //注册Rabbitmq生产者
            builder.RegisterType<RabbitMqProducer>()
                   .InstancePerLifetimeScope();

            //注册Rabbitmq消费者
            builder.RegisterAssemblyTypes(_assembliesToScan.ToArray())
                   .Where(t => t.IsAssignableTo<IHostedService>() && t.IsAssignableTo<BaseRabbitMqConsumer>() && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
