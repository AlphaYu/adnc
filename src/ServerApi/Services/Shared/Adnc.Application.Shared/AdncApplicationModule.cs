using Adnc.Application.Shared.Caching;
using Adnc.Application.Shared.IdGenerater;
using Adnc.Application.Shared.Interceptors;
using Adnc.Application.Shared.Services;
using Adnc.Infra.Caching;
using Adnc.Infra.Caching.Interceptor.Castle;
using Adnc.Infra.Core;
using Adnc.Infra.IRepositories;
using Adnc.Infra.Mapper.AutoMapper;
using Adnc.Infra.Mq;
using Adnc.Infra.Repository;
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
        private readonly Assembly _repoAssemblieToScan;
        private readonly IConfigurationSection _redisSection;
        private readonly string _appModuleName;

        protected AdncApplicationModule(Type modelType, IConfiguration configuration, IServiceInfo serviceInfo)
        {
            _appAssemblieToScan = modelType?.Assembly ?? throw new ArgumentNullException(nameof(modelType));
            _repoAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Repository"));
            _appContractsAssemblieToScan = Assembly.Load(_appAssemblieToScan.FullName.Replace(".Application", ".Application.Contracts"));

            _appModuleName = serviceInfo.ShortName;
            _redisSection = configuration.GetRedisSection();
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
            builder.RegisterType<OperateLogInterceptor>()
                        .InstancePerLifetimeScope();
            builder.RegisterType<OperateLogAsyncInterceptor>()
                        .InstancePerLifetimeScope();

            #endregion register opslog interceptor

            #region register uow interceptor

            //注册操作日志拦截器
            builder.RegisterType<UowInterceptor>()
                        .InstancePerLifetimeScope();
            builder.RegisterType<UowAsyncInterceptor>()
                        .InstancePerLifetimeScope();

            #endregion register uow interceptor

            #region register appservices,interceptors

            //注册应用服务与拦截器
            var interceptors = new List<Type>
            {
                typeof(OperateLogInterceptor)
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

            //注册Id生成器
            builder.RegisterType<WorkerNode>()
                        .AsSelf()
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

            #endregion register cacheservice/bloomfilter
        }

        private void LoadDepends(ContainerBuilder builder)
        {
            builder.RegisterModuleIfNotRegistered(new AdncInfraEventBusModule(_appAssemblieToScan));
            builder.RegisterModuleIfNotRegistered(new AutoMapperModule(_appAssemblieToScan));
            builder.RegisterModuleIfNotRegistered(new AdncInfraCachingModule(_redisSection));
            var modelType = _repoAssemblieToScan.GetTypes().FirstOrDefault(x => x.IsAssignableTo<AdncRepositoryModule>() && !x.IsAbstract);
            builder.RegisterModuleIfNotRegistered(System.Activator.CreateInstance(modelType) as Autofac.Module);
        }
    }
}