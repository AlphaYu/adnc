using Adnc.Application.Shared.HostedServices;
using Adnc.Application.Shared.IdGenerater;
using Adnc.WebApi.Shared;
using Adnc.WebApi.Shared.HostedServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
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
            , Action<SharedServicesRegistration> completedExecute = null)
            where TPermissionHandler : PermissionHandler
        {
            var configuration = services.GetConfiguration();
            var serviceInfo = services.GetServiceInfo();
            var environment = services.GetHostEnvironment();

            services.AddHttpContextAccessor();
            services.AddMemoryCache();

            services.AddHostedService<ChannelConsumersHostedService>();
            services.AddHostedService<CacheAndBloomFilterHostedService>();
            services.AddHostedService(provider =>
            {
                var wokerNode = provider.GetService<WorkerNode>();
                var logger = provider.GetService<ILogger<WorkerNodeHostedService>>();
                var serviceName = serviceInfo.ShortName;
                return new WorkerNodeHostedService(logger, wokerNode, serviceName);
            });

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
    }
}