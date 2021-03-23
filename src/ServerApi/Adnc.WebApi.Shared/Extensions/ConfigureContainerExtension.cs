using System;
using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Adnc.Infr.Consul;
using Adnc.Infr.EfCore;
using Adnc.Infr.Mongo;

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
        public static ContainerBuilder RegisterAdncModules<TAppModule>(this ContainerBuilder builder
            , IConfiguration configuration
            , Action<ContainerBuilder> completedExecute = null)
            where TAppModule : IModule, new()
        {
            //通过配置文件(autofac)注册 var module = new ConfigurationModule(Configuration);builder.RegisterModule(module);
            builder.RegisterModule<AdncInfrMongoModule>();
            builder.RegisterModule<AdncInfrEfCoreModule>();
            var consulUrl = configuration.GetConsulConfig().ConsulUrl;
            builder.RegisterModule(new AdncInfrConsulModule(consulUrl));
            builder.RegisterModule<TAppModule>();

            completedExecute?.Invoke(builder);

            return builder;
        }
    }
}
