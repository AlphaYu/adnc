namespace Adnc.UnitTest.Fixtures;

public class EfCoreDbcontextFixture
{
    public IServiceProvider Container { get; private set; }

    public EfCoreDbcontextFixture()
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
        services.AddAdncInfraDapper();
        services.AddAdncInfraEfCoreMySql(options =>
        {
            options.UseLowerCaseNamingConvention();
            options.UseMySql(FixtureConsts.MySqlConnectString, serverVersion, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });
        Container = services.BuildServiceProvider();
    }
}
