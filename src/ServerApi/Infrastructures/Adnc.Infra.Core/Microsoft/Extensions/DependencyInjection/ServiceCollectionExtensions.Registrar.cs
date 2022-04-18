namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IDependencyRegistrar GetWebApiRegistrar(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IDependencyRegistrar>();
        }
    }
}