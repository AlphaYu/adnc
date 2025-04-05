using Adnc.Shared.WebApi.Registrar;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdnc(this IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, $"{nameof(IServiceCollection)} is null.");
        ArgumentNullException.ThrowIfNull(serviceInfo, $"{nameof(IServiceInfo)} is null.");
        ArgumentNullException.ThrowIfNull(configuration, $"{nameof(IConfiguration)} is null.");

        var apiRegistrarType = serviceInfo.StartAssembly.ExportedTypes.Single(type => type.IsAssignableTo(typeof(AbstractWebApiDependencyRegistrar)) && type.IsNotAbstractClass(true));
        var apiRegistrar = (AbstractWebApiDependencyRegistrar?)Activator.CreateInstance(apiRegistrarType, services, serviceInfo, configuration) ?? throw new InvalidOperationException($"Unable to create an instance of {apiRegistrarType.FullName}");
        apiRegistrar.AddAdncServices();
        return services;
    }
}
