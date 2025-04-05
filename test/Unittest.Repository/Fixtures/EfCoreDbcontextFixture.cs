using Microsoft.Extensions.Configuration;

namespace Adnc.Infra.Unittest.Reposity.Fixtures;

public class EfCoreDbcontextFixture
{
    private IServiceProvider? _container;
    public IServiceProvider Container
    {
        get
        {
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
            return _container ??= new ServiceCollection()
                                            .AddScoped(provider => new Operater { Id = 1000000000001, Account = "unittest", Name = "unittest" })
                                            .AddAdncInfraDapper()
                                            .AddAdncInfraEfCoreMySql(typeof(EntityInfo).Assembly, options =>
                                            {
                                                var connectionString = Configuration["Mysql:ConnectionString"];
                                                options.UseLowerCaseNamingConvention();
                                                options.UseMySql(connectionString, serverVersion, optionsBuilder =>
                                                {
                                                    optionsBuilder.MinBatchSize(4)
                                                                            .MigrationsAssembly(GetType().Assembly.FullName)
                                                                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                                                });
                                            })
                                            .BuildServiceProvider();
        }
    }

    private IConfiguration? _configuration;
    public IConfiguration Configuration
    {
        get
        {
            return _configuration ??= new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .Build();
        }
    }
}
