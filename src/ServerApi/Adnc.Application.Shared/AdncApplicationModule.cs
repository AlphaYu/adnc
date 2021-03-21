using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.DynamicProxy;
using FluentValidation;
using Adnc.Infr.Mq;
using Adnc.Infr.EasyCaching.Interceptor.Castle;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Application.Shared.Mapper;
using Adnc.Core.Shared.Interceptors;
using Adnc.Core.Shared.Entities;

namespace Adnc.Application.Shared
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public abstract class AdncApplicationModule : Autofac.Module
    {
        private readonly Assembly _appAssemblieToScan;
        private readonly Assembly _coreAssemblieToScan;

        protected AdncApplicationModule(Type appModelType)
        {
            _appAssemblieToScan = appModelType.Assembly;
            _coreAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Core"));
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注册依赖模块
            this.LoadDepends(builder);

            //注册操作日志拦截器
            builder.RegisterType<OpsLogInterceptor>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<OpsLogAsyncInterceptor>()
                   .InstancePerLifetimeScope();

            //注册cache拦截器
            builder.RegisterType<EasyCachingInterceptor>()
                   .InstancePerLifetimeScope();

            //注册应用服务与拦截器
            var interceptors = new List<Type>
            {
                typeof(OpsLogInterceptor)
                , typeof(EasyCachingInterceptor)
            };
            if (_coreAssemblieToScan.GetTypes().Any(x => x.IsAssignableTo<AggregateRoot>() && !x.IsAbstract))
            {
                builder.RegisterType<UowInterceptor>()
                       .InstancePerLifetimeScope();
                builder.RegisterType<UowAsyncInterceptor>()
                       .InstancePerLifetimeScope();
                interceptors.Add(typeof(UowInterceptor));
            }
            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                   .Where(t => t.IsAssignableTo<IAppService>())
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(interceptors.ToArray());

            //注册DtoValidators
            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModule(new AdncInfrMqModule(_appAssemblieToScan));
            builder.RegisterModule(new AutoMapperModule(_appAssemblieToScan));
            var modelType = _coreAssemblieToScan.GetTypes().Where(x => x.IsAssignableTo<Autofac.Module>() && !x.IsAbstract).FirstOrDefault();
            builder.RegisterModule(System.Activator.CreateInstance(modelType) as Autofac.Module);
        }
    }
}