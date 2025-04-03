using System.Reflection;
using Adnc.Infra.Repository;
using Adnc.Infra.Repository.EfCore;
using Adnc.Infra.Repository.Interceptor.Castle;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddEntityInfo(this IServiceCollection services, Assembly? assembly)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        if (services.HasRegistered(nameof(AddEntityInfo)))
        {
            return services;
        }

        if (assembly is not null)
        {
            var serviceType = typeof(IEntityInfo);
            var implType = assembly.ExportedTypes.SingleOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
            if (implType is not null)
            {
                services.TryAdd(new ServiceDescriptor(serviceType, implType, ServiceLifetime.Singleton));
            }
            else
            {
                services.TryAdd(new ServiceDescriptor(serviceType, typeof(NullEntityInfo), ServiceLifetime.Singleton));
            }
        }
        return services;
    }

    public static IServiceCollection AddUowInterceptor(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(serviceLifetime, nameof(serviceLifetime));

        if (services.HasRegistered(nameof(AddUowInterceptor)))
        {
            return services;
        }

        services.TryAdd(new ServiceDescriptor(typeof(UowInterceptor), typeof(UowInterceptor), serviceLifetime));
        services.TryAdd(new ServiceDescriptor(typeof(UowAsyncInterceptor), typeof(UowAsyncInterceptor), serviceLifetime));
        return services;
    }

    public static IServiceCollection AddEfRepository(this IServiceCollection services, ServiceLifetime serviceLifetime)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(serviceLifetime, nameof(serviceLifetime));

        if (services.HasRegistered(nameof(AddEfRepository)))
        {
            return services;
        }

        services.TryAdd(new ServiceDescriptor(typeof(IEfRepository<>), typeof(EfRepository<>), serviceLifetime));
        services.TryAdd(new ServiceDescriptor(typeof(IEfBasicRepository<>), typeof(EfBasicRepository<>), serviceLifetime));
        return services;
    }
}
