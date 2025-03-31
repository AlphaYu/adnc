using Microsoft.EntityFrameworkCore;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册EFCoreContext与仓储
    /// </summary>
    protected virtual void AddEfCoreContext()
    {
        AddOperater(Services);

        Services.AddAdncInfraEfCoreMySql(RepositoryOrDomainLayerAssembly, optionsBuilder =>
         {
             var connectionString = Configuration[NodeConsts.Mysql_ConnectionString] ?? throw new ArgumentNullException(nameof(NodeConsts.Mysql_ConnectionString)); ;
             var dbVersion = new MariaDbServerVersion(new Version(11, 7, 2)) as ServerVersion;
             optionsBuilder.UseLowerCaseNamingConvention();
             optionsBuilder.UseMySql(connectionString, dbVersion, mySqlOptions =>
             {
                 mySqlOptions.MinBatchSize(4)
                                                  .MigrationsAssembly(ServiceInfo.MigrationsAssemblyName)
                                                  .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
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
