using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册EFCoreContext与仓储
    /// </summary>
    protected virtual void AddEfCoreContext()
    {
        AddOperater(Services);

        var connectionString = Configuration[NodeConsts.Mysql_ConnectionString] ?? throw new ArgumentNullException($"connectionString is null");
        var versionString = Configuration[NodeConsts.Mysql_ServerVersion] ?? "11.7.2";
        var serverTypeString = Configuration[NodeConsts.Mysql_ServerType] ?? $"{nameof(ServerType.MariaDb)}";
        var serverVersion = Enum.TryParse(serverTypeString, out ServerType serverType) ? ServerVersion.Create(new Version(versionString), serverType) : throw new ArgumentException($"serverTypeString is invalid: {serverTypeString}");
        var migrationsAssemblyName = ServiceInfo.MigrationsAssemblyName;
        var splittingBehavior = QuerySplittingBehavior.SplitQuery;
        Services.AddAdncInfraEfCoreMySql(RepositoryOrDomainLayerAssembly, optionsBuilder =>
         {
             optionsBuilder.UseLowerCaseNamingConvention();
             optionsBuilder.UseMySql(connectionString, serverVersion, mySqlOptions =>
             {
                 mySqlOptions
                 .MinBatchSize(4)
                 .MigrationsAssembly(migrationsAssemblyName)
                 .UseQuerySplittingBehavior(splittingBehavior);
             });
         }, Lifetime);
    }

    protected void AddOperater(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.Add(new ServiceDescriptor(typeof(Operater), provider =>
        {
            var userContext = provider.GetRequiredService<UserContext>();
            return new Operater
            {
                Id = userContext.Id == 0 ? 1000000000000 : userContext.Id,
                Account = userContext.Account.IsNullOrEmpty() ? "system" : userContext.Account,
                Name = userContext.Name.IsNullOrEmpty() ? "system" : userContext.Name
            };

        }, Lifetime));
    }
}
