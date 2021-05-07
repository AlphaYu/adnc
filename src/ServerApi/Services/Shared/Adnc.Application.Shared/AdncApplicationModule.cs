using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Autofac;
using Autofac.Extras.DynamicProxy;
using FluentValidation;
using Adnc.Infra.Mq;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Mapper.AutoMapper;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared.Interceptors;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared;
using Adnc.Infra.Caching;
using Adnc.Application.Shared.IdGeneraterWorkerNode;

namespace Adnc.Application.Shared
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public abstract class AdncApplicationModule : Autofac.Module
    {
        private readonly Assembly _appAssemblieToScan;
        private readonly Assembly _appContractsAssemblieToScan;
        private readonly Assembly _coreAssemblieToScan;
        private readonly IConfigurationSection _redisSection;
        private readonly IConfigurationSection _rabbitMqSection;
        private readonly string _appModuleName;

        protected AdncApplicationModule(Type appModelType, IConfigurationSection redisSection, IConfigurationSection rabbitMqSection)
        {
            _appModuleName = appModelType.Name;
            _appAssemblieToScan = appModelType.Assembly;
            _coreAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Core"));
            _appContractsAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Application.Contracts"));
            _redisSection = redisSection;
            _rabbitMqSection = rabbitMqSection;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //注册依赖模块
            this.LoadDepends(builder);

            //注册UserContext(IUserContext,IOperater)
            builder.RegisterType<UserContext>()
                        .As<IUserContext,IOperater>()
                        .InstancePerLifetimeScope();

            //注册操作日志拦截器
            builder.RegisterType<OpsLogInterceptor>()
                        .InstancePerLifetimeScope();
            builder.RegisterType<OpsLogAsyncInterceptor>()
                        .InstancePerLifetimeScope();

            //注册应用服务与拦截器
            var interceptors = new List<Type>
            {
                typeof(OpsLogInterceptor)
                , typeof(CachingInterceptor)
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
                   .Where(t => t.IsAssignableTo<ICacheService>() && !t.IsAbstract)
                   .AsSelf()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                   .Where(t => t.IsAssignableTo<IAppService>() && !t.IsAbstract)
                   .AsImplementedInterfaces()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                   .InstancePerLifetimeScope()
                   .EnableInterfaceInterceptors()
                   .InterceptedBy(interceptors.ToArray());

            //注册DtoValidators
            builder.RegisterAssemblyTypes(_appContractsAssemblieToScan)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            //注册Id生成器工作节点服务类
            builder.RegisterType<WorkerNode>()
                        .AsSelf()
                        .SingleInstance();
            builder.RegisterType<WorkerNodeHostedService>()
                        .WithParameter("serviceName", _appModuleName.ToLower())
                        .AsImplementedInterfaces()
                        .SingleInstance();
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModule(new AdncInfraEventBusModule(_appAssemblieToScan));
            builder.RegisterModule(new AutoMapperModule(_appAssemblieToScan));
            builder.RegisterModule(new AdncInfraCachingModule(_redisSection));
            var modelType = _coreAssemblieToScan.GetTypes().Where(x => x.IsAssignableTo<AdncCoreModule>() && !x.IsAbstract).FirstOrDefault();
            builder.RegisterModule(System.Activator.CreateInstance(modelType) as Autofac.Module);
        }
    }
}