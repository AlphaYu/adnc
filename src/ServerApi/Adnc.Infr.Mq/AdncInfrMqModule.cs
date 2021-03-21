using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Adnc.Infr.Mq.RabbitMq;
using Microsoft.Extensions.Hosting;

namespace Adnc.Infr.Mq
{
    public class AdncInfrMqModule : Autofac.Module
    {
        private readonly IEnumerable<Assembly> _assembliesToScan;
        public AdncInfrMqModule(IEnumerable<Assembly> assembliesToScan)
        {
            _assembliesToScan = assembliesToScan;
        }

        public AdncInfrMqModule(params Assembly[] assembliesToScan) : this((IEnumerable<Assembly>)assembliesToScan) { }

        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //注册生产者
            builder.RegisterType<RabbitMqProducer>()
                   .InstancePerLifetimeScope();

            //注册消费者
            builder.RegisterAssemblyTypes(_assembliesToScan.ToArray())
                   .Where(t => t.IsAssignableTo<IHostedService>() && t.IsAssignableTo<BaseRabbitMqConsumer>() && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .SingleInstance();
        }
    }
}
