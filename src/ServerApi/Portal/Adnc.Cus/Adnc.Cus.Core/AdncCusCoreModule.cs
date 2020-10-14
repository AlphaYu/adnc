using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Cus.Core.Entities;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Cus.Core
{
    public class AdncCusCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册EntityInfo
            builder.RegisterType<EntityInfo>()
                   .As<IEntityInfo>()
                   .InstancePerLifetimeScope();

            //注册事务拦截器
            builder.RegisterType<UowInterceptor>()
                   .InstancePerLifetimeScope();

            //注册Core服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<ICoreService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(UowInterceptor));
        }
    }
}
