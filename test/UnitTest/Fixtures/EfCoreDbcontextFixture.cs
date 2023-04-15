using Adnc.Infra.Entities;
using Adnc.Infra.IRepositories;
using Adnc.Shared;
using Adnc.UnitTest.TestCases.Repositories.Dtos;
using Adnc.UnitTest.TestCases.Repositories.Entities;

namespace Adnc.UnitTest.Fixtures;

public class EfCoreDbcontextFixture
{
    public IServiceProvider Container { get; private set; }

    public EfCoreDbcontextFixture()
    {
        var services = new ServiceCollection();
        var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
        var userContext =
        services.AddScoped<IEntityInfo, EntityInfo>();
        services.AddScoped(provider =>
        {
            return new UserContext
            {
                Id = 1600000000000,
                Account = "alpha2008",
                Name = "余小猫"
            };
        });
        services.AddAdncInfraAutoMapper(typeof(MapperProfile));
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
