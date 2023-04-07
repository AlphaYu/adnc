using Adnc.Infra.Entities;
using Adnc.Infra.EventBus.Cap;
using Adnc.Infra.IRepositories;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.Fixtures;

public class CoreServiceFixture
{
    public IServiceProvider Container { get; private set; }

    public CoreServiceFixture()
    {
        var services = new ServiceCollection();
        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        services.AddSingleton<IEntityInfo, EntityInfo>();
        services.AddScoped(provider =>
        {
            return new Operater
            {
                Id = 1600000000000,
                Account = "alpha2008",
                Name = "余小猫"
            };
        });

        services.AddAdncInfraEfCoreMySql(options =>
        {
            options.UseLowerCaseNamingConvention();
            options.UseMySql(FixtureConsts.MySqlConnectString, serverVersion, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .CommandTimeout(10)
                                        .EnableRetryOnFailure()
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });

        services.AddSingleton<ICapPublisher, NullCapPublisher>();

        Container = services.BuildServiceProvider();
    }
}