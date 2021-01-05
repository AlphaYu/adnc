using Autofac;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using Adnc.Infr.EfCore;
using Adnc.Infr.Common;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared.Entities;
using Adnc.Infr.EfCore.Interceptors;
using Microsoft.EntityFrameworkCore.Query;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;

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
            containerBuilder.RegisterType<UserContext>()
                            .InstancePerLifetimeScope();

            //注册DbContext Options
            containerBuilder.Register<DbContextOptions>(c =>
            {
                var options = new DbContextOptionsBuilder<AdncDbContext>()
                .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddDebug()))
                .UseMySql(dbstring, mySqlOptions => {
                    mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 8), ServerType.MariaDb));
                    mySqlOptions.CharSet(CharSet.Utf8Mb4);
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

            //注册Adnc.Infr.EfCore
            AdncInfrEfCoreModule.Register(containerBuilder);

            var services = Container = containerBuilder.Build();          
        }

        public void Dispose()
        {
            this.Container?.Dispose();
        }
    }
}
