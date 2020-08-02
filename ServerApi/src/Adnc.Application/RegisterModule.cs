using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Application.Interceptors.OpsLog;
using Adnc.Application.Services;

namespace Adnc.Application
{
    public class RegisterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册操作日志拦截器
            builder.RegisterType<OpsLogInterceptor>();

            //注册服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IAppService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(OpsLogInterceptor));
        }
    }
}