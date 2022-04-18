using Adnc.Cus.Entities;
using Adnc.Infra.EfCore.MySQL;
using Adnc.Infra.Entities;

namespace Adnc.UnitTest.Fixtures
{
    public class EfCoreDbcontextFixture
    {
        public IServiceProvider Container { get; private set; }

        protected EfCoreDbcontextFixture()
        {
            var services = new ServiceCollection();
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
            services.AddSingleton<IEntityInfo, EntityInfo>();
            services.AddScoped<IOperater, Operater>();
            services.AddAdncInfraEfCoreMySql();
            services.AddDbContext<AdncDbContext>(options =>
            {
                options.UseMySql(FixtureConsts.MySqlConnectString, serverVersion, optionsBuilder =>
                {
                    optionsBuilder.MinBatchSize(4)
                                            .CommandTimeout(10)
                                            .EnableRetryOnFailure()
                                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            Container = services.BuildServiceProvider();
        }
    }
}