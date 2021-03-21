using Autofac;
using Adnc.Infr.Mongo.Interfaces;
using Adnc.Core.Shared.IRepositories;

namespace Adnc.Infr.Mongo
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncInfrMongoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册依赖模块
            this.LoadDepends(builder);

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
        
        private void LoadDepends(ContainerBuilder builder)
        {

        }
    }
}