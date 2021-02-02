using Autofac;
using Autofac.Extras.DynamicProxy;
using Adnc.Usr.Core.Entities;
using Adnc.Core.Shared;
using Adnc.Core.Shared.Entities;
using Adnc.Core.Shared.Interceptors;

namespace Adnc.Usr.Core
{
    public class AdncUsrCoreModule : Module
    {
        //拿不到
        //private IServiceProvider _serviceProvider;
        protected override void Load(ContainerBuilder builder)
        {
            //注册EntityInfo
            builder.RegisterType<EntityInfo>()
                   .As<IEntityInfo>()
                   .InstancePerLifetimeScope();

            //注册事务拦截器
            builder.RegisterType<UowInterceptor>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<UowAsyncInterceptor>()
                   .InstancePerLifetimeScope();


            //注册Core服务
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.IsAssignableTo<ICoreService>())
                //.AsImplementedInterfaces()
                //.EnableInterfaceInterceptors()
                .AsSelf()
                .EnableClassInterceptors()
                .InstancePerLifetimeScope()
                .InterceptedBy(typeof(UowInterceptor));
        }
        
        //private void LoadEntityFramwork(ContainerBuilder builder)
        //{

        //    var mysqlConfig = ConfigurationHelper.Current.GetSection("Mysql").Get<MysqlConfig>();

        //    if (mysqlConfig?.WriteDb == null || string.IsNullOrWhiteSpace(mysqlConfig.WriteDb.ConnectionString))
        //        throw new ArgumentException("没有配置写服务器");

        //    if (mysqlConfig?.ReadDbs == null || string.IsNullOrWhiteSpace(mysqlConfig.ReadDbs[0].ConnectionString))
        //        throw new ArgumentException("没有配置读服务器");

        //    var writeDbConfig = mysqlConfig.WriteDb;
        //    var readDbConfig = mysqlConfig.ReadDbs[0];

        //    foreach (var redadDb in mysqlConfig.ReadDbs)
        //    {
        //        //todo 根据访问者算法，计算需要读取那台服务器
        //        if (redadDb.Area == "a")
        //        {
        //            readDbConfig = redadDb;
        //            break;
        //        }
        //    }

        //    //首先注册 options，供 DbContext 服务初始化使用
        //    //containerBuilder.Register(c =>
        //    //{
        //    //    var optionsBuilder = new DbContextOptionsBuilder<BookListDbContext>();
        //    //    optionsBuilder.UseMySql(connectionString, b => b
        //    //        .MigrationsAssembly("BookList.Domain"));
        //    //    return optionsBuilder.Options;
        //    //}).InstancePerLifetimeScope();

        //    ////https://www.cnblogs.com/dudu/p/10398225.html 连接池问题
        //    //builder.Register(c =>
        //    //{
        //    //    var optionsBuilder = new DbContextOptionsBuilder<SystemManageDbContext>();
        //    //    optionsBuilder.UseMySql(writeDbConfig.ConnectionString
        //    //        , mySqlOptions =>  mySqlOptions.ServerVersion(new ServerVersion(new Version(8, 0, 18), ServerType.MySql)));
        //    //    return optionsBuilder.Options;
        //    //})
        //    //.InstancePerLifetimeScope();

        //    // 注册 DbContext
        //    //builder.RegisterType<SystemManageDbContext>()
        //    //    .AsSelf()
        //    //    .InstancePerLifetimeScope();

        //    ////注册其他Repository服务
        //    //builder.RegisterAssemblyTypes(this.ThisAssembly)
        //    //    .Where(t => t.IsClosedTypeOf(typeof(IRepository<>)))
        //    //    .AsImplementedInterfaces()
        //    //    .InstancePerLifetimeScope();
        //}

        //private void LoadMongo(ContainerBuilder builder)
        //{
        //    var mongoDbConfig = ConfigurationHelper.Current.GetSection("MongoDb").Get<MongoConfig>();

        //    if (mongoDbConfig == null || string.IsNullOrWhiteSpace(mongoDbConfig.ConnectionStrings))
        //        return;

        //    var options = Options.Create(new MongoRepositoryOptions
        //    {
        //        ConnectionString = mongoDbConfig.ConnectionStrings
        //        ,
        //        CollectionNamingConvention = (NamingConvention)mongoDbConfig.CollectionNamingConvention
        //        ,
        //        PluralizeCollectionNames = mongoDbConfig.PluralizeCollectionNames
        //    });


        //    builder.RegisterInstance(options).SingleInstance();
        //    builder.RegisterType<MongoContext>().As<IMongoContext>().SingleInstance();
        //    builder.RegisterGeneric(typeof(MongoRepository<>)).As(typeof(IMongoRepository<>)).InstancePerDependency();
        //    builder.RegisterGeneric(typeof(SoftDeletableMongoRepository<>)).As(typeof(ISoftDeletableMongoRepository<>)).InstancePerDependency();


        //    builder.RegisterAssemblyTypes(this.ThisAssembly)
        //        .Where(t => t.IsClosedTypeOf(typeof(IMongoRepository<>)))
        //        .AsImplementedInterfaces()
        //        .InstancePerLifetimeScope();

        //    builder.RegisterAssemblyTypes(this.ThisAssembly)
        //        .Where(t => t.IsClosedTypeOf(typeof(IMongoEntityConfiguration<>)))
        //        .AsImplementedInterfaces()
        //        .InstancePerLifetimeScope();
        //}
    }
}