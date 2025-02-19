using Microsoft.Extensions.Configuration;

namespace Adnc.Infra.Unittest.Reposity.Fixtures;

public class EfCoreDbcontextFixture
{
    public IServiceProvider Container { get; private set; }
    public IConfiguration Configuration { get; private set; }

    public EfCoreDbcontextFixture()
    {
        Configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .Build();

        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        var services = new ServiceCollection();
        services
            .AddScoped(provider => new Operater { Id = 1000000000001, Account = "unittest", Name = "unittest" })
            .AddScoped<IEntityInfo, EntityInfo>()
            .AddAdncInfraAutoMapper(typeof(MapperProfile))
            .AddAdncInfraDapper()
            .AddAdncInfraEfCoreMySql(options =>
             {
                 var connectionString = Configuration["Mysql:ConnectionString"];
                 options.UseLowerCaseNamingConvention();
                 options.UseMySql(connectionString, serverVersion, optionsBuilder =>
                 {
                     optionsBuilder.MinBatchSize(4)
                                             .MigrationsAssembly(GetType().Assembly.FullName)
                                             .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                 });
             });
        Container = services.BuildServiceProvider();
    }
}
