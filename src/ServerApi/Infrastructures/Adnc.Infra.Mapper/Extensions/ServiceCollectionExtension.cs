using Adnc.Infra.Mapper;
using Adnc.Infra.Mapper.AutoMapper;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraAutoMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
    {
        if (services.HasRegistered(nameof(AddAdncInfraAutoMapper)))
            return services;

        services.AddAutoMapper(profileAssemblyMarkerTypes);
        services.AddSingleton<IObjectMapper, AutoMapperObject>();
        return services;
    }

    public static IServiceCollection AddAdncInfraAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (services.HasRegistered(nameof(AddAdncInfraAutoMapper)))
            return services;

        services.AddAutoMapper(assemblies);
        services.AddSingleton<IObjectMapper, AutoMapperObject>();
        return services;
    }
}