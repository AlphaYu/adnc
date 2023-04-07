namespace Adnc.Infra.EfCore;

public class AdncInfraEfCoreModule : Autofac.Module
{
    /// <summary>
    /// Autofac注册
    /// </summary>
    /// <param name="builder"></param>
    protected override void Load(ContainerBuilder builder)
    {
        //注册UOW状态类
        builder.RegisterType<UnitOfWorkStatus>()
               .AsSelf()
               .InstancePerLifetimeScope();

        //注册UOW
        builder.RegisterType<UnitOfWork<AdncDbContext>>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

        //注册ef公共EfRepository
        builder.RegisterGeneric(typeof(EfRepository<>))
               .UsingConstructor(typeof(AdncDbContext))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        //注册ef公共EfBasicRepository
        builder.RegisterGeneric(typeof(EfBasicRepository<>))
               .UsingConstructor(typeof(AdncDbContext))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();

        //注册Repository服务
        builder.RegisterAssemblyTypes(this.ThisAssembly)
               .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
    }

    /// <summary>
    /// Autofac注册,该方法供UnitTest工程使用
    /// </summary>
    /// <param name="builder"></param>
    public static void Register(ContainerBuilder builder)
    {
        new AdncInfraEfCoreModule().Load(builder);
    }
}