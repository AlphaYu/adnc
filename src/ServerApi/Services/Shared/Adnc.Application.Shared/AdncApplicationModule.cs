using Adnc.Application.Shared.Caching;
using Adnc.Application.Shared.HostedServices;
using Adnc.Application.Shared.IdGeneraterWorkerNode;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Interceptors;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Mapper.AutoMapper;
using Adnc.Infra.Mq;
using Autofac;
using Autofac.Extras.DynamicProxy;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

            #region register usercontext,operater

            //注册UserContext(IUserContext,IOperater)
            builder.RegisterType<UserContext>()
                        .As<IUserContext, IOperater>()
                        .InstancePerLifetimeScope();

            #endregion register usercontext,operater

            #region register opslog interceptor

            //注册操作日志拦截器
            builder.RegisterType<OpsLogInterceptor>()
                        .InstancePerLifetimeScope();
            builder.RegisterType<OpsLogAsyncInterceptor>()
                        .InstancePerLifetimeScope();

            #endregion register opslog interceptor

            #region register appservices,interceptors

            //注册应用服务与拦截器
            var interceptors = new List<Type>
            {
                typeof(OpsLogInterceptor)
                , typeof(CachingInterceptor)
                ,typeof(UowInterceptor)
            };

            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                       .Where(t => t.IsAssignableTo<IAppService>() && !t.IsAbstract)
                       .AsImplementedInterfaces()
                       .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                       .InstancePerLifetimeScope()
                       .EnableInterfaceInterceptors()
                       .InterceptedBy(interceptors.ToArray());

            #endregion register appservices,interceptors

            #region register dto validators

            //注册DtoValidators
            builder.RegisterAssemblyTypes(_appContractsAssemblieToScan)
                   .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            #endregion register dto validators

            #region register idgenerater services

            //注册Id生成器工作节点服务类
            builder.RegisterType<WorkerNode>()
                        .AsSelf()
                        .SingleInstance();
            builder.RegisterType<WorkerNodeHostedService>()
                        .WithParameter("serviceName", _appModuleName.ToLower())
                        .AsImplementedInterfaces()
                        .SingleInstance();

            #endregion register idgenerater services

            #region register cacheservice/bloomfilter

            //注册布隆过滤器、cacheservice、cache补偿服务/布隆过滤器初始化服务
            //cacheservcie
            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                       .Where(t => t.IsAssignableTo<ICacheService>() && !t.IsAbstract)
                       .AsSelf().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).InstancePerLifetimeScope()
                       .As<ICacheService>().SingleInstance();
            //bloomfilter
            builder.RegisterAssemblyTypes(_appAssemblieToScan)
                       .Where(t => t.IsAssignableTo<IBloomFilter>() && !t.IsAbstract)
                       .AsImplementedInterfaces()
                       .SingleInstance();
            //bloomfilter factory
            builder.RegisterType<DefaultBloomFilterFactory>().As<IBloomFilterFactory>().SingleInstance();
            //cahce and bloomfiter hostedservice
            builder.RegisterType<CacheAndBloomFilterHostedService>()
                        .AsImplementedInterfaces()
                        .SingleInstance();

            #endregion register cacheservice/bloomfilter
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModule(new AdncInfraEventBusModule(_appAssemblieToScan));
            builder.RegisterModule(new AutoMapperModule(_appAssemblieToScan));
            builder.RegisterModule(new AdncInfraCachingModule(_redisSection));
            var modelType = _coreAssemblieToScan.GetTypes().FirstOrDefault(x => x.IsAssignableTo<AdncCoreModule>() && !x.IsAbstract);
            builder.RegisterModule(System.Activator.CreateInstance(modelType) as Autofac.Module);
        }
    }
}