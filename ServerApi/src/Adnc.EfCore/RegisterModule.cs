using System;
using Autofac;
using Humanizer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Adnc.Common;
using Adnc.Common.Helper;
using Adnc.Core.IRepositories;
using Adnc.Infr.EfCore.Repositories;

namespace  Adnc.Infr.EfCore
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册ef公共Repository
            builder.RegisterGeneric(typeof(EfRepository<>))
                .UsingConstructor(typeof(AdncDbContext))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //注册Repository服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}