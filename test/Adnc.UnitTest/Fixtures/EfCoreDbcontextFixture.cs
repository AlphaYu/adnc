using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Adnc.Infr.EfCore;
using Adnc.Infr.Common;
using Adnc.Cus.Core;
using Adnc.Cus.Core.CoreServices;
using Adnc.Cus.Core.Entities;
using Adnc.Cus.Core.EventBus;
using Adnc.Core.Shared;
using Adnc.Infr.EfCore.Repositories;
using Adnc.Core.Shared.IRepositories;
using Adnc.Core.Shared.Entities;

namespace Adnc.UnitTest.Fixtures
{
    public class EfCoreDbcontextFixture: IDisposable
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
            containerBuilder.RegisterType<UserContext>()
                            .InstancePerLifetimeScope();

            //注册DbContext Options
            containerBuilder.Register<DbContextOptions>(c =>
            {
                return new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, mySqlOptions => mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql)))
                .Options;
            }).InstancePerLifetimeScope();


            //注册EntityInfo
            containerBuilder.RegisterType<EntityInfo>()
                            .As<IEntityInfo>()
                            .InstancePerLifetimeScope();

            //注册DbContext
            containerBuilder.RegisterType<AdncDbContext>()
                            .InstancePerLifetimeScope();

            //注册UOW
            containerBuilder.RegisterType<UnitOfWork<AdncDbContext>>()
                            .As<IUnitOfWork>()
                            .InstancePerLifetimeScope();


            //注册ef公共Repository
            containerBuilder.RegisterGeneric(typeof(EfRepository<>))
                            .UsingConstructor(typeof(AdncDbContext))
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            //注册Repository服务
            containerBuilder.RegisterAssemblyTypes(Assembly.Load("Adnc.Infr.EfCore"))
                            .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            var services = Container = containerBuilder.Build();          
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}
