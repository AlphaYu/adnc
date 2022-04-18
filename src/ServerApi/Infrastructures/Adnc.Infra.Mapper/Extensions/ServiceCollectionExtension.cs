﻿using Adnc.Infra.Mapper;
using Adnc.Infra.Mapper.AutoMapper;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAdncInfraAutoMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
    {
        services.AddAutoMapper(profileAssemblyMarkerTypes);
        services.AddSingleton<IObjectMapper, AutoMapperMapperImpl>();
        return services;
    }

    public static IServiceCollection AddAdncInfraAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddAutoMapper(assemblies);
        services.AddSingleton<IObjectMapper, AutoMapperMapperImpl>();
        return services;
    }
}