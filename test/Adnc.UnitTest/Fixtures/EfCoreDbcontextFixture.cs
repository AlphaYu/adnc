using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Cus.Core.Entities;
using Adnc.Infra.EfCore;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace Adnc.UnitTest.Fixtures
{
    public class EfCoreDbcontextFixture : IDisposable
    {
        public IContainer Container { get; private set; }

        public EfCoreDbcontextFixture()
        {
            var containerBuilder = new ContainerBuilder();
            //内存数据库
            //var option = new DbContextOptionsBuilder<MyDbContext>().UseInMemoryDatabase("My.D3").Options;
            //MyDbContext context = new MyDbContext(option);            //InitializeDbForTests  初始化测试数据
            //new TestDataBuilder(context).Build();
            //注入
            //Server.ContentRootPath = Path.GetFullPath(@"..\..\..\..\..\") + @"src\My.D3";
            //IConfigurationRoot configuration = AppConfigurationHelper.Get(Server.ContentRootPath);

            //builder.RegisterType<SimpleDbContextProvider<MyDbContext>>().As<IDbContextProvider<MyDbContext>>().InstancePerLifetimeScope();
            //var assemblysServices = Assembly.Load("My.D3.Application");
            //builder.RegisterAssemblyTypes(assemblysServices).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(DbFixture).GetTypeInfo().Assembly);
            //var config = new MapperConfiguration(c => c.AddProfile(typeof(AdncProfile)));
            //IMapper mapper = config.CreateMapper();
            //builder.RegisterInstance(mapper).As<IMapper>();
            //var userContext = new UserContext() { ID = 1600000000000, Account = "alpha2008", Name = "余小猫" };

            var dbstring = "Server=193.112.75.77;Port=13308;database=adnc_cus_dev;uid=root;pwd=alpha.netcore;";

            //注册操作用户
            containerBuilder.RegisterType<Operater>()
                        .As<IOperater>()
                        .InstancePerLifetimeScope();

            //注册DbContext Options
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
            containerBuilder.Register<DbContextOptions>(c =>
            {
                return new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, serverVersion, mySqlOptions =>
                {
                    mySqlOptions.MinBatchSize(4);
                })
                .Options;
            }).InstancePerLifetimeScope();

            //注册EntityInfo
            containerBuilder.RegisterType<EntityInfo>()
                            .As<IEntityInfo>()
                            .InstancePerLifetimeScope();

            //注册DbContext2
            containerBuilder.RegisterType<AdncDbContext>()
                            .InstancePerLifetimeScope();

            //注册Adnc.Infra.EfCore
            AdncInfraEfCoreModule.Register(containerBuilder);

            Container = containerBuilder.Build();
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}