using System;
using System.Linq;
using System.Reflection;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Adnc.Infr.Consul;
using Adnc.Infr.EfCore;
using Adnc.Infr.Mongo;
using Adnc.WebApi.Shared;
using Adnc.Application.Shared; 

namespace Autofac
{
    public static class ConfigureContainerExtension
    {
        /// <summary>
        /// 统一注册Adnc.WebApi通用模块
        /// </summary>
        /// <typeparam name="TAppModule"></typeparam>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAdncModules(this ContainerBuilder builder
            , IConfiguration configuration
            , ServiceInfo serverInfo
            , Action<ContainerBuilder> completedExecute = null)
        {
            builder.RegisterModule<AdncInfrMongoModule>();
            builder.RegisterModule<AdncInfrEfCoreModule>();
            builder.RegisterModule(new AdncInfrConsulModule(configuration.GetConsulConfig().ConsulUrl));

            var appAssembly = Assembly.Load(serverInfo.AssemblyFullName.Replace("WebApi", "Application"));
            var appModelType = appAssembly.GetTypes().Where(m =>
                                                       m.FullName != null
                                                       && typeof(AdncApplicationModule).IsAssignableFrom(m)
                                                       && !m.IsAbstract).FirstOrDefault();
            builder.RegisterModule(Activator.CreateInstance(appModelType) as  IModule);

            completedExecute?.Invoke(builder);

            return builder;
        }
    }
}
