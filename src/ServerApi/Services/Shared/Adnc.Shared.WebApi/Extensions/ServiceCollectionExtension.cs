﻿namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// 统一注册Adnc.WebApi通用服务
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
        //_srvRegistration.AddHangfire();

        completedExecute?.Invoke(_srvRegistration);

        return services;
    }
}