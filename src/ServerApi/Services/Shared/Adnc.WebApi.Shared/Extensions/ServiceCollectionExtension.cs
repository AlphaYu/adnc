using Adnc.Application.Shared.Caching;
using Adnc.WebApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SkyApm.Diagnostics.CAP;
using SkyApm.Diagnostics.MongoDB;
using SkyApm.Utilities.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 统一注册Adnc.Webpi通用服务
        /// </summary>
        /// <typeparam name="TPermissionHandler"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <param name="serviceInfo"></param>
        /// <param name="completedExecute"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdncServices<TPermissionHandler>(this IServiceCollection services
            , IConfiguration configuration
            , IWebHostEnvironment environment
            , ServiceInfo serviceInfo
            , Action<SharedServicesRegistration> completedExecute = null)
            where TPermissionHandler : PermissionHandler
        {
            services.AddSingleton(serviceInfo);
            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            var _srvRegistration = new SharedServicesRegistration(configuration, services, environment, serviceInfo);
            _srvRegistration.Configure();
            _srvRegistration.AddControllers();
            _srvRegistration.AddJWTAuthentication();
            _srvRegistration.AddAuthorization<TPermissionHandler>();
            _srvRegistration.AddCors();
            _srvRegistration.AddHealthChecks();
            _srvRegistration.AddEfCoreContext();
            _srvRegistration.AddMongoContext();
            _srvRegistration.AddSwaggerGen();

            completedExecute?.Invoke(_srvRegistration);

            return services;
        }

        /// <summary>
        /// 统一注册Adnc.Webpi通用链路跟踪
        /// </summary>
        /// <param name="services"></param>
        /// <param name="completedExecute"></param>
        /// <returns></returns>
        public static IServiceCollection AddAdncSkyApms(this IServiceCollection services
            , Action<SkyApmExtensions> completedExecute = null)
        {
            var amps = services.AddSkyApmExtensions();
            amps.AddCaching();
            amps.AddCap();
            amps.AddMongoDB();
            completedExecute?.Invoke(amps);
            return services;
        }
    }
}