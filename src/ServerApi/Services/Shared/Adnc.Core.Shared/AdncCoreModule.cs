using System;
using System.Linq;
using System.Reflection;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Interceptors;
using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Autofac;
using Autofac.Extras.DynamicProxy;

namespace Adnc.Core.Shared
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public abstract class AdncCoreModule : Autofac.Module
    {
        private readonly Assembly _assemblieToScan;
        public AdncCoreModule(Type modelType)
        {
            _assemblieToScan = modelType.Assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注册EntityInfo
            builder.RegisterAssemblyTypes(_assemblieToScan)
                   .Where(t => t.IsAssignableTo<IEntityInfo>() && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            //注册事件发布者
            builder.RegisterType<CapPublisher>()
                   .As<IEventPublisher>()
                   .SingleInstance();

            //注册服务
            var registionBuilder = builder.RegisterAssemblyTypes(_assemblieToScan)
                   .Where(t => t.IsAssignableTo<ICoreService>())
                   .AsSelf()
                   .InstancePerLifetimeScope();
            if (!_assemblieToScan.GetTypes().Any(x => x.IsAssignableTo<AggregateRoot>() && !x.IsAbstract))
            {
                builder.RegisterType<UowInterceptor>()
                       .InstancePerLifetimeScope();
                builder.RegisterType<UowAsyncInterceptor>()
                       .InstancePerLifetimeScope();
                registionBuilder.EnableClassInterceptors()
                                .InterceptedBy(typeof(UowInterceptor));
            }
        }
    }
}