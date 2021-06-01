using Adnc.Core.Shared;
using Adnc.Cus.Core;
using Adnc.Infra.EfCore;
using Adnc.Infra.EventBus.Cap;
using Autofac;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

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
            containerBuilder.RegisterType<Operater>()
                        .As<IOperater>()
                        .InstancePerLifetimeScope();

            //注册DbContext Options
            containerBuilder.Register<DbContextOptions>(c =>
            {
                var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));

                return new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, serverVersion).Options;
            })
                .InstancePerLifetimeScope();

            //注册DbContext
            containerBuilder.RegisterType<AdncDbContext>().InstancePerLifetimeScope();

            //注册事件发布者
            containerBuilder.RegisterType<NullCapPublisher>()
                       .As<ICapPublisher>().SingleInstance();

            //注册Adnc.Infra.EfCore
            AdncInfrEfCoreModule.Register(containerBuilder);

            //注册 Adnc.Cus.Core
            AdncCusCoreModule.Register(containerBuilder);

            Container = containerBuilder.Build();
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}