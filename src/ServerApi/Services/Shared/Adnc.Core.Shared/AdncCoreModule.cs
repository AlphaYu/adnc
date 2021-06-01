using Adnc.Core.Shared.Entities;
using Adnc.Infra.EventBus;
using Adnc.Infra.EventBus.Cap;
using Autofac;
using System;
using System.Linq;
using System.Reflection;

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
            builder.RegisterAssemblyTypes(_assemblieToScan)
                   .Where(t => t.IsAssignableTo<ICoreService>())
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }
    }
}