using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Adnc.Infr.EfCore.Interceptors;
using Adnc.Application;
using Adnc.Core;
using Adnc.Core.IRepositories;
using Adnc.Application.Services;
using Adnc.Infr.EfCore;
using Adnc.Common.Models;

namespace Adnc.UnitTest
{
    public class EfCoreDbcontextFixture: IDisposable
    {
        public IContainer Container { get; private set; }

        public EfCoreDbcontextFixture()
        {
            var builder = new ContainerBuilder();
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

            string dbstring = "Server=localhost;database=AlphaNetCore;uid=root;pwd=alpha1;";
            var option = new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, mySqlOptions => mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql)))
                .AddInterceptors(new CustomCommandInterceptor()).Options;

            var config = new MapperConfiguration(c => c.AddProfile(typeof(AdncProfile)));

            IMapper mapper = config.CreateMapper();

            var userContext = new UserContext() { ID=2};
            var dbContext = new AdncDbContext(option, userContext);

            builder.RegisterInstance(userContext).As<UserContext>();
            builder.RegisterInstance(mapper).As<IMapper>();
            builder.RegisterInstance(dbContext).As<AdncDbContext>();

            //builder.Register(c => new SystemManageDbContext(option, new UserContext())).InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork<AdncDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();


            //注册Repository服务
            var assemblysRepositories = Assembly.Load("Andc.EfCore");
            builder.RegisterAssemblyTypes(assemblysRepositories)
                .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //注册AppServices服务
            assemblysRepositories = Assembly.Load("Andc.Application");
            builder.RegisterAssemblyTypes(assemblysRepositories)
                .Where(t => t.IsAssignableTo<IAppService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //注册Domain服务
            assemblysRepositories = Assembly.Load("Andc.Core");
            builder.RegisterAssemblyTypes(assemblysRepositories)
                .Where(t => t.IsAssignableTo<IDomainService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var services = Container = builder.Build();          
        }

        public void Dispose()
        {

        }
    }
}
