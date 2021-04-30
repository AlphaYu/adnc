using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;
using Adnc.Application.Shared.RpcServices;
using Adnc.WebApi.Shared;

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

            var policies = _srvRegistration.GenerateDefaultRefitPolicies();
            var authServerAddress = (environment.IsProduction() || environment.IsStaging()) ? "adnc.usr.webapi" : "http://localhost:5010";
            _srvRegistration.AddRpcService<IAuthRpcService>(authServerAddress, policies);

            var maintServiceAddress = (environment.IsProduction() || environment.IsStaging()) ? "adnc.maint.webapi" : "http://localhost:5020";
            _srvRegistration.AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

            completedExecute?.Invoke(_srvRegistration);

            return services;
        }
    }
}
