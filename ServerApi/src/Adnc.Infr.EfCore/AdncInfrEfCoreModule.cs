using Autofac;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.EfCore.Repositories;
using Adnc.Core.Shared;
using Microsoft.EntityFrameworkCore;

namespace  Adnc.Infr.EfCore
{
    public class AdncInfrEfCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册UOW
            builder.RegisterType(typeof(UnitOfWork<AdncDbContext>))
                   .As(typeof(IUnitOfWork))
                   .InstancePerLifetimeScope();

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