using Adnc.Application.Shared;
using Adnc.Infra.Consul;
using Adnc.Infra.EfCore;
using Adnc.Infra.Mongo;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace Autofac
{
    public static class ConfigureContainerExtension
    {
        /// <summary>
        /// 统一注册Adnc.WebApi通用模块
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <param name="serverInfo"></param>
        /// <param name="completedExecute"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAdncModules(this ContainerBuilder builder, IServiceCollection services, Action<ContainerBuilder> completedExecute = null)
        {
            var configuration = services.GetConfiguration();
            var serviceInfo = services.GetServiceInfo();

            builder.RegisterModuleIfNotRegistered<AdncInfraMongoModule>();
            builder.RegisterModuleIfNotRegistered<AdncInfraEfCoreModule>();
            var consulUrl = configuration.GetConsulSection().Get<ConsulConfig>().ConsulUrl;
            builder.RegisterModuleIfNotRegistered(new AdncInfraConsulModule(consulUrl));

            var appAssembly = Assembly.Load(serviceInfo.AssemblyFullName.Replace("WebApi", "Application"));
            var appModelType = appAssembly.GetTypes().FirstOrDefault(m =>
                                                       m.FullName != null
                                                       && typeof(AdncApplicationModule).IsAssignableFrom(m)
                                                       && !m.IsAbstract);
            builder.RegisterModuleIfNotRegistered(Activator.CreateInstance(appModelType, configuration, serviceInfo) as IModule);

            completedExecute?.Invoke(builder);

            return builder;
        }
    }
}