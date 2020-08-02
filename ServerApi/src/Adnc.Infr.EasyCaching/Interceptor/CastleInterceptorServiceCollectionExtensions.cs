using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;
using EasyCaching.Core.Configurations;
using EasyCaching.Core.Interceptor;

namespace Adnc.Infr.EasyCaching.Interceptor.Castle
{
    /// <summary>
    /// Castle interceptor service collection extensions.
    /// </summary>
    public static class CastleInterceptorServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the castle interceptor.
        /// </summary>
        /// <returns>The castle interceptor.</returns>
        /// <param name="services">Services.</param>
        /// <param name="options">Easycaching Interceptor config</param>        
        public static void ConfigureCastleInterceptor(this IServiceCollection services, Action<EasyCachingInterceptorOptions> options)
        {
            services.TryAddSingleton<IEasyCachingKeyGenerator, CustomEasyCachingKeyGenerator>();
            services.Configure(options);
        }

        /// <summary>
        /// Configures the castle interceptor.
        /// </summary>
        /// <param name="builder">Container Builder.</param>
        public static void ConfigureCastleInterceptor(this ContainerBuilder builder)
        {
            //var assembly = isCalling ? Assembly.GetCallingAssembly() : Assembly.GetExecutingAssembly();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder.RegisterType<EasyCachingInterceptor>();

            builder.RegisterAssemblyTypes(assemblies)
                .Where(t => !t.IsAbstract && t.GetInterfaces().SelectMany(x => x.GetMethods()).Any(
                    y => y.CustomAttributes.Any(data =>
                                    typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType)
                                )))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(EasyCachingInterceptor));
        }

        internal static void ConfigureCastleInterceptorForTest(this ContainerBuilder builder)
        {
            var assembly = Assembly.GetCallingAssembly();

            builder.RegisterType<EasyCachingInterceptor>();

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => !t.IsAbstract && t.GetInterfaces().SelectMany(x => x.GetMethods()).Any(
                    y => y.CustomAttributes.Any(data =>
                                    typeof(EasyCachingInterceptorAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType)
                                )))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(EasyCachingInterceptor));
        }
    }
}
