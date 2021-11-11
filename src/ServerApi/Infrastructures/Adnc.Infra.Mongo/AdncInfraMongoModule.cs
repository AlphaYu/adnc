using Adnc.Infra.IRepositories;

using Adnc.Infra.Mongo.Interfaces;
using Autofac;

namespace Adnc.Infra.Mongo
{
    /// <summary>
    /// MongoDb模块注册
    /// </summary>
    public class AdncInfraMongoModule : Module
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
                   .Where(t => t.IsClosedTypeOf(typeof(IMongoEntityConfiguration<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}