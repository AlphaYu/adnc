using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Warehouse.Core;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;

namespace Adnc.Warehouse.Application
{
    public class AdncWarehouseApplicationModule : Module
    {

        public AdncWarehouseApplicationModule()
        {
        }

        public AdncWarehouseApplicationModule(IServiceCollection services, IConfiguration configuration)
            :base()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注册依赖模块
            this.LoadDepends(builder);

            //注册RabitMq生产者
            builder.RegisterType<RabbitMqProducer>()
                   .SingleInstance();

            //注册操作日志拦截器
            builder.RegisterType<OpsLogInterceptor>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<OpsLogAsyncInterceptor>()
                   .InstancePerLifetimeScope();

            //注册cache拦截器
            builder.RegisterType<EasyCachingInterceptor>()
                   .InstancePerLifetimeScope();

            //注册服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<IAppService>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(OpsLogInterceptor),typeof(EasyCachingInterceptor));
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModule<AdncWarehouseApplicationModule>();
        }
    }
}