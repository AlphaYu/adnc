using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.EventBus.RabbitMq;
using Autofac;
using DotNetCore.CAP;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adnc.Infra.Mq
{
    public class AdncInfraEventBusModule : Autofac.Module
    {
        private readonly IEnumerable<Assembly> _assembliesToScan;

        public AdncInfraEventBusModule(IEnumerable<Assembly> assembliesToScan)
            => _assembliesToScan = assembliesToScan;

        public AdncInfraEventBusModule(params Assembly[] assembliesToScan) 
            : this((IEnumerable<Assembly>)assembliesToScan)
        {
        }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //注册Rabbitmq生产者
            builder.RegisterType<RabbitMqProducer>().SingleInstance();

            //注册Rabbitmq消费者
            builder.RegisterAssemblyTypes(_assembliesToScan.ToArray())
                       .Where(t => t.IsAssignableTo<IHostedService>() && t.IsAssignableTo<BaseRabbitMqConsumer>() && !t.IsAbstract)
                       .AsImplementedInterfaces()
                       .SingleInstance();

            //注册事件发布者
            builder.RegisterType<CapPublisher>().As<IEventPublisher>().SingleInstance();
        }

        protected virtual void Load(IServiceCollection service)
        {
            var implements = _assembliesToScan.SelectMany(x=>x.GetTypes())
                                                                         .Where(imp => typeof(ICapSubscribe).IsAssignableFrom(imp) && !imp.IsAbstract)
                                                                         .ToList();
            implements.ForEach(loader => service.AddSingleton(loader));
        }
    }
}