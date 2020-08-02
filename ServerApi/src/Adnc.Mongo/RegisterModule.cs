using Autofac;
using Adnc.Infr.Mongo.Interfaces;
using Adnc.Core.IRepositories;
using Adnc.Infr.Mongo;

namespace Adnc.Infr.Mongo
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            //注册mongo公共Repository
            builder.RegisterGeneric(typeof(MongoRepository<>))
                .UsingConstructor(typeof(IMongoContext))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //注册Repository服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t=>t.IsClosedTypeOf(typeof(IMongoEntityConfiguration<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}