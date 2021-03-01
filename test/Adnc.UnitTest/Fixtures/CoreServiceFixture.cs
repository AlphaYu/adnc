using Autofac;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Adnc.Infr.EfCore;
using Adnc.Infr.Common;
using Adnc.Cus.Core;
using DotNetCore.CAP;
using Adnc.Infr.EventBus;

namespace Adnc.UnitTest.Fixtures
{
    public class CoreServiceFixture : IDisposable
    {
        public IContainer Container { get; private set; }

        public CoreServiceFixture()
        {
            var containerBuilder = new ContainerBuilder();

            var dbstring = "Server=193.112.75.77;Port=13308;database=adnc_cus_dev;uid=root;pwd=alpha.netcore;";

            //注册操作用户
            containerBuilder.RegisterType<UserContext>()
                            .InstancePerLifetimeScope();

            //注册DbContext Options
            containerBuilder.Register<DbContextOptions>(c =>
            {
                return new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, mySqlOptions => mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb)))
                .Options;
            }).InstancePerLifetimeScope();

            //注册DbContext
            containerBuilder.RegisterType<AdncDbContext>()
                            .InstancePerLifetimeScope();

            //注册事件发布者
            containerBuilder.RegisterType<NullCapPublisher>()
                   .As<ICapPublisher>()
                   .SingleInstance();

            //注册Adnc.Infr.EfCore
            AdncInfrEfCoreModule.Register(containerBuilder);

            //注册 Adnc.Cus.Core
            AdncCusCoreModule.Register(containerBuilder);

            var services = Container = containerBuilder.Build();
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}
