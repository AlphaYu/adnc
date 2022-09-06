using Adnc.Infra.Repository.Mongo;
using Adnc.Infra.Repository.Mongo.Configuration;
using Adnc.Infra.Repository.Mongo.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册EFCoreContext与仓储
    /// </summary>
    protected virtual void AddEfCoreContextWithRepositories()
    {
        var serviceType = typeof(IEntityInfo);
        var implType = RepositoryOrDomainLayerAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        if (implType is null)
            throw new NullReferenceException(nameof(IEntityInfo));
        else
            Services.AddSingleton(serviceType, implType);

        Services.AddScoped(provider =>
        {
            var userContext = provider.GetRequiredService<UserContext>();
            return new Operater
            {
                Id = userContext.Id == 0 ? 1600000000000 : userContext.Id,
                Account = userContext.Account.IsNullOrEmpty() ? "system" : userContext.Account,
                Name = userContext.Name.IsNullOrEmpty() ? "system" : userContext.Name
            };
        });

        AddEfCoreContext();
    }

    /// <summary>
    /// 注册EFCoreContext
    /// </summary>
    protected virtual void AddEfCoreContext()
    {
        var mysqlConfig = MysqlSection.Get<MysqlConfig>();
        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        Services.AddAdncInfraEfCoreMySql(options =>
        {
            options.UseLowerCaseNamingConvention();
            options.UseMySql(mysqlConfig.ConnectionString, serverVersion, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(ServiceInfo.StartAssembly.GetName().Name.Replace("WebApi", "Migrations"))
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });

            if (this.IsDevelopment())
            {
                //options.AddInterceptors(new DefaultDbCommandInterceptor())
                options.LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors();
            }
            //替换默认查询sql生成器,如果通过mycat中间件实现读写分离需要替换默认SQL工厂。
            //options.ReplaceService<IQuerySqlGeneratorFactory, AdncMySqlQuerySqlGeneratorFactory>();
        });
    }

    /// <summary>
    /// 注册MongoContext与仓储
    /// </summary>
    protected virtual void AddMongoContextWithRepositries(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var mongoConfig = MongoDbSection.Get<MongoConfig>();
        Services.AddAdncInfraMongo<MongoContext>(options =>
        {
            options.ConnectionString = mongoConfig.ConnectionString;
            options.PluralizeCollectionNames = mongoConfig.PluralizeCollectionNames;
            options.CollectionNamingConvention = (NamingConvention)mongoConfig.CollectionNamingConvention;
        });
    }
}