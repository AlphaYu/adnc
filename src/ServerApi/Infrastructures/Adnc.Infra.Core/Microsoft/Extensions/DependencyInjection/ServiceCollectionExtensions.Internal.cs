namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
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
}