namespace Microsoft.Extensions.DependencyInjection;

public static class AdncRepositoryServiceCollectionExtension
{
    public static IServiceCollection AddAdncRepositry(this IServiceCollection services)
    {
        services.Scan(scan => scan
        .FromCallingAssembly()
        .AddClasses(c => c.AssignableTo<IEntityInfo>())
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        return services;
    }
}