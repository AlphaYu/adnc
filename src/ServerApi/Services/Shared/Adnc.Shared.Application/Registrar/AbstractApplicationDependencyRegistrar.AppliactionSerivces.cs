namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
{
    protected ProxyGenerator CastleProxyGenerator => new();
    protected virtual List<Type> DefaultInterceptorTypes => new() { typeof(OperateLogInterceptor), typeof(CachingInterceptor), typeof(UowInterceptor) };

    /// <summary>
    /// 注册Application服务
    /// </summary>
    protected virtual void AddAppliactionSerivcesWithInterceptors(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var appServiceType = typeof(IAppService);
        var serviceTypes = ContractsLayerAssembly.GetExportedTypes().Where(type => type.IsInterface && type.IsAssignableTo(appServiceType)).ToList();
        serviceTypes.Remove(appServiceType);
        var lifetime = ServiceLifetime.Scoped;
        if (serviceTypes.IsNullOrEmpty())
            return;
        serviceTypes.ForEach(serviceType =>
        {
            var implType = ApplicationLayerAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is null)
                return;

            Services.Add(new ServiceDescriptor(implType, implType, lifetime));

            var serviceDescriptor = new ServiceDescriptor(serviceType, provider =>
            {
                var interceptors = DefaultInterceptorTypes.ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToArray();
                var target = provider.GetService(implType);
                var interfaceToProxy = serviceType;
                var proxy = CastleProxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, target, interceptors);
                return proxy;
            }, lifetime);
            Services.Add(serviceDescriptor);
        });
    }

    /// <summary>
    /// 注册Application的IHostedService服务
    /// </summary>
    protected virtual void AddApplicaitonHostedServices()
    {
        var serviceType = typeof(IHostedService);
        var implTypes = ApplicationLayerAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implTypes.ForEach(implType =>
        {
            Services.AddSingleton(serviceType, implType);
        });
    }
}