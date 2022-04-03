using Adnc.Infra.Mapper;
using Adnc.Infra.Mapper.AutoMapper;
using AutoMapper;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
    {
        services.AddAutoMapper(profileAssemblyMarkerTypes);
        services.AddScoped<IObjectMapper, AutoMapperMapperImpl>();
        return services;
    }
}