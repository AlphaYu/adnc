using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    private static readonly ConcurrentDictionary<string, char> s_RegisteredModels = new();

    public static bool HasRegistered(this IServiceCollection _, string modelName) => !s_RegisteredModels.TryAdd(modelName.ToLower(), '1');

    public static IServiceCollection ReplaceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Replace(ServiceDescriptor.Singleton(configuration));
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
        if (hostBuilderContext?.Configuration is not null)
        {
            var instance = hostBuilderContext.Configuration as IConfigurationRoot;
            if (instance is not null)
                return instance;
        }

        return services.GetSingletonInstance<IConfiguration>();
    }

    internal static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        where T : class
    {
        var instance = services.FirstOrDefault(d => d.ServiceType == typeof(T))?.ImplementationInstance;
        if (instance is null)
            return null;

        return (T)instance;
    }

    internal static T GetSingletonInstance<T>(this IServiceCollection services)
        where T : class
    {
        var instance = GetSingletonInstanceOrNull<T>(services);
        if (instance is null)
            throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
        return instance;
    }
}