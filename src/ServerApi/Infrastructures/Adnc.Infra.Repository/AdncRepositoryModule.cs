using Adnc.Infra.Entities;
using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Adnc.Infra.Repository
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    public abstract class AdncRepositoryModule : Autofac.Module
    {
        private readonly Assembly _assemblieToScan;

        protected AdncRepositoryModule(Type modelType)
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
        }
    }
}