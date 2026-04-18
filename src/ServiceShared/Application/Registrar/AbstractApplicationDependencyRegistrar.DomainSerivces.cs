namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// Registers domain services.
    /// </summary>
    protected virtual void AddDomainSerivces<TDomainService>()
        where TDomainService : class
    {
        var serviceType = typeof(TDomainService);
        var implTypes = RepositoryOrDomainLayerAssembly.ExportedTypes.Where(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true)).ToList();
        implTypes?.ForEach(implType =>
        {
            Services.Add(new ServiceDescriptor(implType, implType, Lifetime));
        });
    }
}
