﻿using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    protected static List<Type> DefaultInterceptorTypes => [typeof(OperateLogInterceptor), typeof(CachingInterceptor), typeof(UowInterceptor)];

    /// <summary>
    /// 注册Application服务
    /// </summary>
    protected virtual void AddAppliactionSerivcesWithInterceptors(Action<IServiceCollection>? action = null)
    {
        action?.Invoke(Services);

        var appServiceType = typeof(IAppService);
        var serviceTypes = ContractsLayerAssembly.GetExportedTypes().Where(type => type.IsInterface && type.IsAssignableTo(appServiceType)).ToList();
        if (serviceTypes is null)
        {
            return;
        }

        //注册拦截器
        Services.AddScoped<OperateLogInterceptor>();
        Services.AddScoped<OperateLogAsyncInterceptor>();
        Services.AddScoped<UowInterceptor>();
        Services.AddScoped<UowAsyncInterceptor>();
        serviceTypes.ForEach(serviceType =>
        {
            var implType = ApplicationLayerAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is null)
            {
                return;
            }

            Services.AddScoped(implType);
            Services.TryAddSingleton(new ProxyGenerator());
            Services.AddScoped(serviceType, provider =>
            {
                var interfaceToProxy = serviceType;
                var target = provider.GetService(implType);
                var interceptors = DefaultInterceptorTypes.ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToArray();
                var proxyGenerator = provider.GetRequiredService<ProxyGenerator>();
                var proxy = proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, target, interceptors);
                return proxy;
            });
        });

        //注册Application的IHostedService服务
        var serviceType = typeof(IHostedService);
        var implTypes = ApplicationLayerAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implTypes.ForEach(implType =>
        {
            Services.AddSingleton(serviceType, implType);
        });
    }
}