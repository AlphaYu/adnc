using Adnc.Shared.Application.Contracts.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    private static Assembly appAssembly;

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

        services.AddHttpContextAccessor();
        services.AddMemoryCache();

        var _srvRegistration = new SharedServicesRegistration(configuration, services, serviceInfo);
        _srvRegistration.Configure();
        _srvRegistration.AddControllers();
        _srvRegistration.AddJWTAuthentication();
        _srvRegistration.AddAuthorization<TPermissionHandler>();
        _srvRegistration.AddCors();
        _srvRegistration.AddHealthChecks();
        _srvRegistration.AddSwaggerGen();

        var assembly = services.GetApplicationAssembly();
        if (assembly != null)
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

    /// <summary>
    /// 获取Application程序集
    /// </summary>
    /// <returns></returns>
    public static Assembly GetApplicationAssembly(this IServiceCollection services)
    {
        if (appAssembly == null)
        {
            //var appAssemblyName = serviceInfo.AssemblyFullName.Replace("WebApi", "Application");
            //var appAssembly = Assembly.Load(appAssemblyName);
            var serviceInfo = services.GetServiceInfo();
            var appAssemblyPath = serviceInfo.AssemblyLocation.Replace(".WebApi.dll", ".Application.dll");
            appAssembly = Assembly.LoadFrom(appAssemblyPath);
        }
        return appAssembly;
    }
}