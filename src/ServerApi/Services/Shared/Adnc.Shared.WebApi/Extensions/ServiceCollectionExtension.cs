using Adnc.Shared.Application.Contracts.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

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
        where TPermissionHandler : AbstractPermissionHandler
    {
        var configuration = services.GetConfiguration();
        var serviceInfo = services.GetServiceInfo();

        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        var _srvRegistration = new SharedServicesRegistration(configuration, services, serviceInfo);
        _srvRegistration.Configure();
        _srvRegistration.AddControllers();
        _srvRegistration.AddAuthentication();
        _srvRegistration.AddAuthorization<TPermissionHandler>();
        _srvRegistration.AddCors();
        _srvRegistration.AddHealthChecks();
        _srvRegistration.AddSwaggerGen();

        var appAssembly = serviceInfo.GetApplicationAssembly();
        if (appAssembly != null)
        {
            var modelType = appAssembly.GetTypes()
                                                 .FirstOrDefault(
                                                   m => m.FullName != null
                                                   && m.IsAssignableTo(typeof(IAdncServiceCollection))
                                                   && !m.IsAbstract
                                                  );
            if (modelType != null)
            {
                var adncServiceCollection = System.Activator.CreateInstance(modelType, services) as IAdncServiceCollection;
                adncServiceCollection.AddAdncServices();
            }
        }

        completedExecute?.Invoke(_srvRegistration);
        return services;
    }
}