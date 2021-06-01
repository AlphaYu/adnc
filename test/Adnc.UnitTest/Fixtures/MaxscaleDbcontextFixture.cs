using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Cus.Core.Entities;
using Adnc.Infra.EfCore;
using Adnc.Infra.EfCore.Interceptors;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

namespace Adnc.UnitTest.Fixtures
{
    public class MaxscaleDbcontextFixture : IDisposable
    {
        public IContainer Container { get; private set; }

        public MaxscaleDbcontextFixture()
        {
            var containerBuilder = new ContainerBuilder();
            //maxscale连接地址
            //var dbstring = "Server=193.112.75.77;Port=14006;database=adnc_cus;uid=adnc;pwd=123abc;";
            var dbstring = "server=193.112.75.77;port=14006;user=adnc;password=123abc;database=adnc_cus";

            //注册操作用户
            containerBuilder.RegisterType<Operater>()
                        .As<IOperater>()
                        .InstancePerLifetimeScope();

            //注册DbContext Options
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 4));
            containerBuilder.Register<DbContextOptions>(c =>
            {
                var options = new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, serverVersion, mySqlOptions =>
                {
                })
                .AddInterceptors(new CustomCommandInterceptor())
                .Options;
                return options;
            }).InstancePerLifetimeScope();

            //注册EntityInfo
            containerBuilder.RegisterType<EntityInfo>()
                            .As<IEntityInfo>()
                            .InstancePerLifetimeScope();

            //注册DbContext
            containerBuilder.RegisterType<AdncDbContext>()
                            .InstancePerLifetimeScope();

            //注册Adnc.Infra.EfCore
            AdncInfraEfCoreModule.Register(containerBuilder);

            var services = Container = containerBuilder.Build();
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}