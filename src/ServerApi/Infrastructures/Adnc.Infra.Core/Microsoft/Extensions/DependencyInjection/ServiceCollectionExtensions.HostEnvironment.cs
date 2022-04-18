namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IHostEnvironment GetHostEnvironment(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();
            if (hostBuilderContext?.HostingEnvironment != null)
            {
                return hostBuilderContext.HostingEnvironment;
            }

            return services.GetSingletonInstance<IHostEnvironment>();
        }
    }
}