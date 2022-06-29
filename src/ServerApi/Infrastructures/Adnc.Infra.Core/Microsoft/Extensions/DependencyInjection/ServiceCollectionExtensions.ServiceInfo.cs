using Adnc.Infra.Core.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceInfo GetServiceInfo(this IServiceCollection services)
        {
            return services.GetSingletonInstance<IServiceInfo>();
        }
    }
}