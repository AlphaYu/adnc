## ADNC 如何使用仓储 - 读写分离

[GitHub 仓库地址](https://github.com/alphayu/adnc)
当单库系统遇到性能瓶颈时，读写分离通常是首选的优化手段之一。绝大多数系统中，读操作远多于写操作，大量耗时的读请求容易导致锁表，进而影响写入。因此，读写分离尤为重要。

关于 `EF Core` 如何实现读写分离，常见方案包括注册 `DbContextFactory`，分别注入 `ReadDbContext` 和 `WriteDbContext`，或动态切换数据库连接字符串。

这些方法虽然可以实现基础的读写分离，但若需支持从库状态监控（如从库宕机）、主备自动切换、从库负载均衡、将特定 SQL 路由到指定从库、部分表无需读写分离（如基础数据表）等高级特性，或未来涉及分片（分库、分表）和分布式事务，仅靠业务代码实现将极为复杂且难以保证稳定性。

是否有更优雅的解决方案？数据库中间件或许是更佳选择，`EF Core` 同样可以通过中间件优雅实现读写分离。

## 为什么要使用中间件
- 采用客户端直连（C# 代码实现）进行读写分离，虽然省去中间件转发环节，查询性能可能略有提升，但需要了解后端部署细节。主备切换、库迁移等场景下，客户端需感知并调整数据库连接信息。若通过中间件转发，则客户端无需关心后端细节、连接维护及主从切换，均由中间件自动处理，业务端可专注于业务逻辑开发。
- 在大多数生产环境中，数据库是性能瓶颈。读写分离通常是首要优化手段；若仍无法满足需求，则需要分片（分库、分表）。数据分片后，应用需处理多数据源；若无数据库中间件，应用需要自行应对数据源切换、事务处理、数据聚合等问题，容易重复实现通用能力并显著增加开发复杂度。引入数据库中间件后，应用可专注业务处理，通用的数据聚合、事务、数据源切换等交由中间件负责。
- 国内主流厂商及云平台均有自研数据库中间件。


## 主流免费开源中间件简介
目前社区中成熟、免费且持续维护的数据库中间件有：

- Mycat
- ShardingSphere-Proxy
- ProxySQL
- MaxScale

### Mycat 
- 官网：http://www.mycat.org.cn/
- 开发语言：`Java`
- 是否支持分片：支持
- 支持的数据库：MySQL/MariaDB、Oracle、DB2、SQL Server、PostgreSQL
- 路由规则：事务包裹的 SQL 全部走写库，无事务包裹时可通过 Hint 设置读写库，其余功能通过配置实现。
- 简介：Mycat 于 2013 年从阿里 Cobar 分离重构，持续更新。2015 年已有电信、银行级客户应用。Mycat 是四款中间件中支持数据库类型最多、功能最全的。推荐阅读 <a href="http://www.mycat.org.cn/document/mycat-definitive-guide.pdf" target="_blank" rel="noopener">Mycat 权威指南</a>，以了解主从复制、分库分表规则及优缺点。需注意 2016 年后官方文档、Wiki 及 mycat-web 更新缓慢；若文档与配套工具能持续完善，Mycat 将是优先选择。

### ShardingSphere-Proxy
- 官网：http://shardingsphere.apache.org/index_zh.html
- 开发语言：`Java`
- 是否支持分片：支持
- 支持的数据库：MySQL/MariaDB、PostgreSQL
- 路由规则：同一线程且同一数据库连接中，若发生写操作，后续读操作也会路由至写库；可通过 Hint 强制指定路由，其余功能通过配置实现。
- 简介：ShardingSphere 提供多款产品，.NET 场景推荐 ShardingSphere-Proxy。该项目由当当网开源，京东深度参与，2020 年 4 月成为 Apache 顶级项目。相关文档以官方内容为主，案例相对较少，后续可持续关注。

### ProxySQL
- 官网：https://proxysql.com/
- 开发语言：`C++`
- 是否支持分片：支持
- 支持的数据库：MySQL/MariaDB
- 简介：ProxySQL 是一款成熟的 MySQL/MariaDB 数据库中间件，官网文档详尽，案例丰富。其路由规则灵活，可基于用户、schema 或单条 SQL 语句定制，并支持通过 Hint 配合路由规则指定路由，是值得考虑的选择。

### MaxScale
- 官网：https://mariadb.com/kb/en/maxscale/
- 开发语言：`C`
- 是否支持分片：不支持
- 支持的数据库：MySQL/MariaDB
- 路由规则：事务包裹的 SQL 全部走写库，无事务包裹时可通过 Hint 设置读写库，其余功能通过配置实现。
- 简介：MaxScale 由 MariaDB 官方开发，功能成熟，文档详尽，案例丰富。其提供多种过滤器，如 HintFilter、NamedServerFilter（可指定表全部路由至写库）、TopFilter（可将最慢的 N 条查询路由到指定读库）等。其高可用性配置在同类产品中较为完善，具体请参考官方文档。


综上，四款中间件均具备灵活的路由规则和丰富的配置选项，能够实现高效的读写分离，而非简单的主从切换。它们均支持 Hint 语法及后端数据库监控。对于 C# 代码端，仅需设置 SQL 的 Hint，其余交由中间件处理。

> Hint 作为 SQL 的补充语法，在关系型数据库中具有重要作用。它允许用户通过特定语法影响 SQL 执行方式，实现特殊优化。
>
> 例如，maxscale 指定读写库的 Hint 用法：
>
> ```sql
> -- maxscale route to master
> SELECT * FROM table1;
> ```

## Adnc 选择 MaxScale 的原因
- Adnc 后端数据库为 MariaDB，Maxscale 由 MariaDB 官方开发，兼容性最佳
- Maxscale 官方文档详尽，网络案例丰富，版本控制规范
- Maxscale 提供最丰富的数据库集群高可用性配置
- Maxscale 路由规则灵活，支持多种自定义 Filter
- Maxscale 搭配 MariaDB 性能优异（部分原因是其不支持分库/分表）
- Maxscale 不支持分库、分表，但 Adnc 作为微服务框架，已天然实现按服务分库。分表需求暂无法满足，但过度设计并非最佳实践，适合自身场景最重要。


## EF Core 生成 MaxScale Hint 的实现
实现读写分离需部署数据库集群，并基于 maxscale 中间件，还需安装 maxscale。

EF Core 的 `TagWith` 方法详见 [官方文档](https://docs.microsoft.com/en-us/ef/core/querying/tags)。

```csharp
public static class RepositoryConsts
{
    public static readonly string MYCAT_ROUTE_TO_MASTER = "#mycat:db_type=master";
    public static readonly string MAXSCALE_ROUTE_TO_MASTER = "maxscale route to master";
}

public abstract class BaseRepository<TDbContext, TEntity> : IEfRepository<TEntity>
       where TDbContext : DbContext
       where TEntity : EfEntity
{
    public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false) where TrdEntity : EfEntity
    {
        var dbSet = DbContext.Set<TrdEntity>().AsNoTracking();
        if (writeDb)
            // 读操作路由到写库
            return dbSet.TagWith(RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER);
        return dbSet;
    }

    public virtual async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
    {
        if (writeDb)
            // 集成 Dapper 实现复杂查询，读操作路由到写库
            sql = string.Concat("/* ", RepositoryConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
        return await DbContext.Database.GetDbConnection().QueryAsync<TResult>(sql, param, null, commandTimeout, commandType);
    }
}
```
基于 maxscale 的代码实现如上，数据库连接字符串与直连数据库一致，仅需将端口改为 maxscale 的端口。

## EF Core 生成 Mycat Hint 的实现
下面介绍 mycat Hint 的生成方式，同样需先部署数据库集群，并基于 mycat 中间件。

EF Core 生成 Mycat Hint 稍复杂，`TagWith` 方法生成的 Hint 如下：

```sql
-- #mycat:db_type=master
SELECT * FROM TABLE1
```

而 Mycat 要求的格式为：

```sql
/*#mycat:db_type=master*/
SELECT * FROM TABLE1
```
以 `Pomelo.EntityFrameworkCore.MySql` 为例，EFCore 提供 `IQuerySqlGeneratorFactory` 接口，`Pomelo` 的 `MySqlQuerySqlGeneratorFactory` 实现该接口，`Create()` 方法负责生成具体的 `QuerySqlGenerator`，即查询 SQL 的生成。

主要需完成三步：
- 新建工厂类 `AdncMySqlQuerySqlGeneratorFactory`，继承 `MySqlQuerySqlGeneratorFactory` 并重写 `Create()` 方法。示例代码如下
```csharp
namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal
{
    /// <summary>
    /// adnc sql生成工厂类
    /// </summary>
    public class AdncMySqlQuerySqlGeneratorFactory : MySqlQuerySqlGeneratorFactory
    {
        private readonly QuerySqlGeneratorDependencies _dependencies;
        private readonly MySqlSqlExpressionFactory _sqlExpressionFactory;
        private readonly IMySqlOptions _options;

        public AdncMySqlQuerySqlGeneratorFactory(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            ISqlExpressionFactory sqlExpressionFactory,
            IMySqlOptions options) : base(dependencies, sqlExpressionFactory, options)
        {
            _dependencies = dependencies;
            _sqlExpressionFactory = (MySqlSqlExpressionFactory)sqlExpressionFactory;
            _options = options;
        }

        /// <summary>
        /// 重写QuerySqlGenerator
        /// </summary>
        /// <returns></returns>
        public override QuerySqlGenerator Create()
        {
            var result = new AdncQuerySqlGenerator(_dependencies, _sqlExpressionFactory, _options);
            return result;
        }
    }
}
```

- 新建Sql生成类`AdncQuerySqlGenerator`继承`QuerySqlGenerator`,覆写两个方法。
```csharp
namespace Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal
{
    /// <summary>
    /// adnc sql 生成类
    /// </summary>
    public class AdncQuerySqlGenerator : MySqlQuerySqlGenerator
    {
        protected readonly Guid ContextId;
        private bool _isQueryMaseter = false;

        public AdncQuerySqlGenerator(
            [NotNull] QuerySqlGeneratorDependencies dependencies,
            [NotNull] MySqlSqlExpressionFactory sqlExpressionFactory,
            [CanBeNull] IMySqlOptions options)
            : base(dependencies, sqlExpressionFactory, options)
        {
            ContextId = Guid.NewGuid();
        }

        /// <summary>
        /// 获取IQueryable的tags
        /// </summary>
        /// <param name="selectExpression"></param>
        protected override void GenerateTagsHeaderComment(SelectExpression selectExpression)
        {
            if (selectExpression.Tags.Contains(EfCoreConsts.MyCAT_ROUTE_TO_MASTER))
            {
                _isQueryMaseter = true;
                selectExpression.Tags.Remove(EfCoreConsts.MyCAT_ROUTE_TO_MASTER);
            }
            base.GenerateTagsHeaderComment(selectExpression);
        }

        /// <summary>
        /// pomelo最终生成的sql
        /// 该方法主要是调试用
        /// </summary>
        /// <param name="selectExpression"></param>
        /// <returns></returns>
        public override IRelationalCommand GetCommand(SelectExpression selectExpression)
        {
            var command = base.GetCommand(selectExpression);
            return command;
        }

        /// <summary>
        /// 在pomelo生成查询sql前，插入mycat注解
        /// 该注解的意思是从写库读取数据
        /// </summary>
        /// <param name="selectExpression"></param>
        /// <returns></returns>
        protected override Expression VisitSelect(SelectExpression selectExpression)
        {
            if (_isQueryMaseter)
                Sql.Append(string.Concat("/*", EfCoreConsts.MyCAT_ROUTE_TO_MASTER, "*/ "));

            return base.VisitSelect(selectExpression);
        }
    }
}
```
- 注册DbContext时替换`Pomelo`的SQL生成工厂
```csharp
/// <summary>
/// 注册EfcoreContext
/// </summary>
public virtual void AddEfCoreContext()
{
    _services.AddDbContext<AdncDbContext>(options =>
    {
       options.UseMySql(_mysqlConfig.ConnectionString, mySqlOptions =>
       {
          mySqlOptions.ServerVersion(new ServerVersion(new Version(10, 5, 4), ServerType.MariaDb));
          mySqlOptions.CharSet(CharSet.Utf8Mb4);
       });
       //替换默认查询sql生成器,如果通过mycat中间件实现读写分离需要替换默认SQL工厂。
       options.ReplaceService<IQuerySqlGeneratorFactory, AdncMySqlQuerySqlGeneratorFactory>();
    });
}
```
- 使用方法

```csharp

public class EfCoreConsts
{
    public const string MyCAT_ROUTE_TO_MASTER = "#mycat:db_type=master";
    public const string MAXSCALE_ROUTE_TO_MASTER = "maxscale route to master";
}

public abstract class BaseRepository<TDbContext, TEntity> : IEfRepository<TEntity>
       where TDbContext : DbContext
       where TEntity : EfEntity
{
        public virtual IQueryable<TrdEntity> GetAll<TrdEntity>(bool writeDb = false) where TrdEntity : EfEntity
        {
            var dbSet = DbContext.Set<TrdEntity>().AsNoTracking();
            if (writeDb)
                //读操作路由到写库
                return dbSet.TagWith(EfCoreConsts.MyCAT_ROUTE_TO_MASTER);
            return dbSet;
        }

        public virtual async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
        {
            if (writeDb)
                //这个方法集成了dapper实现复杂查询,读操作路由到写库
                sql = string.Concat("/* ", EfCoreConsts.MyCAT_ROUTE_TO_MASTER, " */", sql);
            return await DbContext.Database.GetDbConnection().QueryAsync<TResult>(sql, param, null, commandTimeout, commandType);
        }
}
```
基于 Mycat 的代码实现如上，数据库连接字符串与直连数据库一致，仅需将端口改为 Mycat 的端口。

---
—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
