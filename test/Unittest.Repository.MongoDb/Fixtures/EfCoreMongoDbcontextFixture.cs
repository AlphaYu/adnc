using Microsoft.Extensions.Configuration;

namespace Adnc.Infra.Unittest.Reposity.MongoDb.Fixtures;

public class EfCoreMongoDbcontextFixture
{
    public IServiceProvider Container { get; private set; }
    public IConfiguration Configuration { get; private set; }

    public EfCoreMongoDbcontextFixture()
    {
        Configuration = new ConfigurationBuilder()
                                            .SetBasePath(Directory.GetCurrentDirectory())
                                            .AddJsonFile("appsettings.json", optional: true)
                                            .Build();

        var services = new ServiceCollection();
        services
            .AddScoped(provider => new Operater { Id = 1000000000001, Account = "unittest", Name = "unittest" })
            .AddAdncInfraEfCoreMongoDb(GetType().Assembly, options =>
             {
                 var connectionString = Configuration["MongoDb:ConnectionString"];
                 var databaseName = Configuration["MongoDb:DataBase"];
                 options.UseLowerCaseNamingConvention();
                 options.UseMongoDB(connectionString, databaseName);
             });
        Container = services.BuildServiceProvider();
    }
}
