﻿namespace Adnc.Shared.Domain;

/// <summary>
/// Autofac注册
/// </summary>
public abstract class AdncDomainModule : Autofac.Module
{
    private readonly Assembly _assemblieToScan;

    protected AdncDomainModule(Type modelType)
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

        //注册服务
        builder.RegisterAssemblyTypes(_assemblieToScan)
               .Where(t => t.IsAssignableTo<IDomainService>())
               .AsSelf()
               .InstancePerLifetimeScope();
    }
}