﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Ord.Domain;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Ord.Application
{
    /// <summary>
    /// 订单中心Autofac注册模块
    /// </summary>
    public class AdncOrdApplicationModule : Module
    {
        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="builder"></param>
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

            //注册事务拦截器
            builder.RegisterType<UowInterceptor>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<UowAsyncInterceptor>()
                   .InstancePerLifetimeScope();

            //注册cache拦截器
            builder.RegisterType<EasyCachingInterceptor>()
                   .InstancePerLifetimeScope();

            //注册App服务
            //拦截器执行顺序OpsLogInterceptor=>EasyCachingInterceptor=>UowInterceptor
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                   .Where(t => t.IsAssignableTo<IAppService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(typeof(OpsLogInterceptor)
                    , typeof(EasyCachingInterceptor)
                    , typeof(UowInterceptor));

            //注册领域事件订阅者
            //builder.RegisterType<ShelfToProductAllocatedEventSubscirber>()
            //       .AsSelf()
            //       .SingleInstance();
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModule<AdncOrdDomainModule>();
        }
    }
}