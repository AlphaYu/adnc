## ADNC 如何使用仓储 - CodeFirst

[GitHub 仓库地址](https://github.com/alphayu/adnc)
本文主要介绍在 `ADNC` 框架中，如何将实体映射到数据库。示例采用 Code First 模式；如有需要，也可使用 DB First 模式。
本文所有操作均以 `Adnc.Cust` 微服务为例，其他微服务的定义方式一致。

- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/" title="如何使用仓储(二)-工作单元">如何使用仓储(二)-工作单元</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/" title="如何使用仓储(三)-CodeFirst">如何使用仓储(三)-CodeFirst</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sql/" title="如何使用仓储(四)-撸SQL">如何使用仓储(四)-撸SQL</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/" title="如何使用仓储(五)-切换数据库类型">如何使用仓储(五)-切换数据库类型</a>

## 如何映射
### 第一步，创建实体
在 `Adnc.Cus.Repository` 工程的 `Entities` 目录下创建实体（例如 `Customer`）。如果采用 DDD 模式，则实体通常位于 `Adnc.Xxx.Domain` 工程中。

> 若采用经典三层模式开发，实体需直接或间接继承 `EfEntity` 类。
> 若采用 DDD 架构模式开发，实体需直接或间接继承 `AggregateRoot` 或 `DomainEntity`。
> `Adnc.Cust` 采用经典三层模式；DDD 架构模式可参考 `Adnc.Ord` / `Adnc.Whse`，实现方式一致。

`EfEntity` 经典三层开发模式中所有实体类的基类 

```csharp
//重要派生类
public abstract class EfBasicAuditEntity : EfEntity, IBasicAuditInfo
{
}
//重要派生类
public abstract class EfFullAuditEntity : EfEntity, IFullAuditInfo
{
}
```
创建一个实体类完整代码如下：
```csharp
namespace Adnc.Cus.Entities
{
    /// <summary>
    /// 客户表
    /// </summary>
    public class Customer : EfFullAuditEntity
    {
        public string Account { get; set; }

        public string Nickname { get; set; }

        public string Realname { get; set; }

        public virtual CustomerFinance FinanceInfo { get; set; }

        public virtual ICollection<CustomerTransactionLog> TransactionLogs { get; set; }
    }
}
```
### 第二步，定义映射关系
在`Entities/Config`目录下创建映射关系类，如`CustomerConfig`  
> 通过`fluentapi`创建映射关系。

```csharp
namespace Adnc.Cus.Entities.Config
{
    public class CustomerConfig : EntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.HasOne(d => d.FinanceInfo).WithOne(p => p.Customer).HasForeignKey<CustomerFinance>(d => d.Id).OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.TransactionLogs).WithOne().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(CustConsts.Account_MaxLength);

            builder.Property(x => x.Nickname).IsRequired().HasMaxLength(CustConsts.Nickname_MaxLength);

            builder.Property(x => x.Realname).IsRequired().HasMaxLength(CustConsts.Realname_Maxlength);
        }
    }
}
```
很多示例中`CustomerConfig`是直接继承`IEntityTypeConfiguration<TEntity>`这个接口。我这里稍微封装了下。创建了一个`EntityTypeConfiguration<TEntity>`抽象类并实现了`IEntityTypeConfiguration<TEntity>`接口。然后我们实体关系映射类再继承这个抽象类。这样做主要是为了统一处理一些公共特性字段的映射。如软删除、并发列映射等等，代码如下。
```csharp
public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
   where TEntity : Entity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        var entityType = typeof(TEntity);
        ConfigureKey(builder, entityType);
        ConfigureConcurrency(builder, entityType);
        ConfigureQueryFilter(builder, entityType);
    }

    protected virtual void ConfigureKey(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnOrder(1).ValueGeneratedNever();
    }

    protected virtual void ConfigureConcurrency(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(IConcurrency).IsAssignableFrom(entityType))
            builder.Property("RowVersion").IsRequired().IsRowVersion().ValueGeneratedOnAddOrUpdate();
    }

    protected virtual void ConfigureQueryFilter(EntityTypeBuilder<TEntity> builder, Type entityType)
    {
        if (typeof(ISoftDelete).IsAssignableFrom(entityType))
        {
            builder.Property("IsDeleted")
                       .HasDefaultValue(false)
                       .HasColumnOrder(2);
            builder.HasQueryFilter(d => !EF.Property<bool>(d, "IsDeleted"));
        }
    }
}
```
### 第三步，创建EntityInfo类
在`Entities`创建一个`EntityInfo`类，并实现`IEntityInfo`接口。这个类每个工程只需要定义一个，是公用的。我的项目模板生成工具写好后，项目模板生成工具生成的项目会包含这个类。`GetEntitiesTypeInfo()`方法就是在当前程序集中查找继承了`EfEntity`的类，并放入集合中。

```csharp
namespace Adnc.Cus.Entities
{
    public class EntityInfo : AbstractEntityInfo
    {
        public override IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo()
        {
            return base.GetEntityTypes(this.GetType().Assembly).Select(x => new EntityTypeInfo() { Type = x, DataSeeding = default });
        }
    }
}
```
### 第四步，注入`EntityInfo`到容器，`services.AddEfCoreContextWithRepositories()`扩展方法中会统一注册。
```csharp
public abstract class AbstractApplicationDependencyRegistrar : IDependencyRegistrar
{
    protected virtual void AddEfCoreContextWithRepositories(Action<IServiceCollection> action = null)
    {
        action?.Invoke(Services);

        var serviceType = typeof(IEntityInfo);
        var implType = RepositoryOrDomainAssembly.ExportedTypes.FirstOrDefault(type => type.IsAssignableTo(serviceType) && type.IsNotAbstractClass(true));
        if (implType is null)
            throw new NullReferenceException(nameof(IEntityInfo));
        else
            Services.AddSingleton(serviceType, implType);
        //注册其他服务
    }
}
```
### 第五步，生成迁移代码并更新到数据库
- 设置`Adnc.Cus.WebApi`为启动项目(迁移命令会从这个工程读取数据库连接串)
- 在VS工具中打开Nuget的程序包管理器控制台(工具=>Nuget包管理器=>程序包管理器控制台)
- 设置“程序包管理器控制台”默认项目为`Adnc.Cus.Migrations`
- 执行命令`add-migration Update2021030401`。执行成功后，会在`Adnc.Cus.Migrations`工程的`Migrations`目录下生成迁移文件。
- 执行命令`update-database`,更新到数据库。

---

## 实体如何关联数据库

我们看`Adnc.Infra.EfCore.MySQL`工程的`AdncDbContext`类的源码。

```csharp
namespace Adnc.Infra.EfCore.MySQL
{
    public class AdncDbContext : DbContext
    {
        private readonly Operater _operater;
        private readonly IEntityInfo _entityInfo;
        private readonly UnitOfWorkStatus _unitOfWorkStatus;

        //构造函数注入IEntityInfo接口
        public AdncDbContext([NotNull] DbContextOptions options, Operater operater,IEntityInfo entityInfo, UnitOfWorkStatus unitOfWorkStatus)
            : base(options)
        {
            _operater = operater;
            _entityInfo = entityInfo;
            _unitOfWorkStatus = unitOfWorkStatus;
            Database.AutoTransactionsEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4 ");
            //添加实体到模型上下文
            var entityInfos = _entityInfo.GetEntitiesTypeInfo().ToList();
            Guard.Checker.NotNullOrAny(entityInfos, nameof(entityInfos));
            foreach (var info in entityInfos)
            {
                if (info.DataSeeding.IsNullOrEmpty())
                    modelBuilder.Entity(info.Type);
                else
                    modelBuilder.Entity(info.Type).HasData(info.DataSeeding);
            }
            
            //从程序集加载fuluentapi加载配置文件
            var assembly = entityInfos.FirstOrDefault().Type.Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            //这里做两件事情
            //1、统一把表名，列名转换成小写。
            //2、读取实体的注释<Summary>部分填充Comment
            var types = entityInfos.Select(x => x.Type);
            var entityTypes = modelBuilder.Model.GetEntityTypes().Where(x => types.Contains(x.ClrType)).ToList();
            entityTypes.ForEach(entityType =>
            {
                modelBuilder.Entity(entityType.Name, buider =>
                {
                    var typeSummary = entityType.ClrType.GetSummary();
                    buider.ToTable(entityType.ClrType.Name.ToLower()).HasComment(typeSummary);

                    var properties = entityType.GetProperties().ToList();
                    properties.ForEach(property =>
                    {
                        var memberSummary = entityType.ClrType.GetMember(property.Name).FirstOrDefault().GetSummary();
                        buider.Property(property.Name)
                            .HasColumnName(property.Name.ToLower())
                            .HasComment(memberSummary);
                    });
                });
            });
        }
    }
}
```

—— 完 ——
