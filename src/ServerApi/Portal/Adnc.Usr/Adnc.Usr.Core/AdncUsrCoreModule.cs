using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Usr.Core
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public class AdncUsrCoreModule : Module
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
            builder.RegisterType<UowAsyncInterceptor>()
                   .InstancePerLifetimeScope();


            //注册Core服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                   .Where(t => t.IsAssignableTo<ICoreService>())
                   .AsSelf()
                   .EnableClassInterceptors()
                   .InstancePerLifetimeScope()
                   .InterceptedBy(typeof(UowInterceptor));
        }
    }
}