using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Warehouse.Domain.Entities;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Interceptors;
using Adnc.Warehouse.Domain.Events;
using Adnc.Core.Shared.Events;

namespace Adnc.Warehouse.Domain
{
    public class AdncWarehouseCoreModule : Module
    {
        /// <summary>
        /// Autofac注册
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //注册EntityInfo
            builder.RegisterType<EntityInfo>()
                   .As<IEntityInfo>()
                   .InstancePerLifetimeScope();

            //注册Core服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                   .Where(t => t.IsAssignableTo<ICoreService>())
                   .AsSelf()
                   .InstancePerLifetimeScope();

            //注册事件发布者
            builder.RegisterType<EventPublisher>()
                   .As<IEventPublisher>()
                   .SingleInstance();
        }

        /// <summary>
        /// Autofac注册,该方法供UnitTest工程使用
        /// </summary>
        /// <param name="builder"></param>
        public static void Register(ContainerBuilder builder)
        {
            new AdncWarehouseCoreModule().Load(builder);
        }
    }
}
