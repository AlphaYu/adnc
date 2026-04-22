## 前言
&ensp; &ensp; 我们都知道当单库系统遇到性能瓶颈时，读写分离是首要优化手段之一。因为绝大多数系统读的比例远高于写的比例，并且大量耗时的读操作容易引起锁表导致无发写入数据，这时读写分离就更加重要了。

&ensp; &ensp; `EF Core`如何通过代码实现读写分离，我们可以搜索到很多案例。总结起来一种方法是注册一个`DbContextFactory`，读操作注入`ReadDcontext`，写操作注入`WriteDbcontext`；另外一种是动态修改数据库连接串。

&ensp; &ensp; 以上无论哪种方法，实现简单粗暴的读写分离功能也不复杂。但是如果需要实现从库状态监测（从库宕机）、主备自动切换（主库宕机）、从库灵活的负载均衡配置、耗时查询的SQL路由到指定的从库、指定一些表不需要读写分离（如:基础数据表）等等，随着系统的数据量增加，以后还会涉及到分片（分库，分表），分片后又会涉及到分布式事务，以上这些如果通过业务代码去实现那需要费太多脑子且稳定性是个大问题。

&ensp; &ensp; 有没有更优雅的法案？中间件或许是个不错的选择，`EF Core`也一样可以很好的基于中间实现读写分离。

## 为什么要使用中间件
- 读写分离采用客户端直连方案(c#代码实现)，因为少了一层中间件转发，查询性能可能会稍微好一点。但是这种方案，由于要了解后端部署细节，所以在出现主备切换、库迁移等操作的时候，客户端都需要感知到，并且需要调整数据库连接信息。如果通过中间件转发，客户端不需要关注后端细节、连接维护、主从切换等工作，都由中间件完成。这样可以让业务端只专注于业务逻辑开发。
- 绝大部分生产项目，性能的瓶颈都在数据库。实现读写分离是解决性能瓶颈的首要手段之一。然而当读写分离还不能解决时，接下来手段就是分片（分库、分表）。数据被分到多个分片数据库后，应用如果需要读取数据，就要需要处理多个数据源的数据。如果没有数据库中间件，那么应用将直接面对分片集群，数据源切换、事务处理、数据聚合都需要应用直接处理，原本该是专注于业务的应用，将会花大量的工作来处理分片后的问题，最重要的是每个应用处理将是完全的重复造轮子。所以有了数据库中间件，应用只需要集中与业务处理，大量的通用的数据聚合，事务，数据源切换都由中间件来处理。
- 国内各大厂、各个云平台都有自己的数据库中间件。  

## 几款免费开源中间件介绍
目前社区成熟的、免费开源并且还在维护的中间件有`mycat`、`shardingsphere-proxy`、`proxysql`、`maxscale`。

### mycat 
- 官网：http://www.mycat.org.cn/
- 开发语言：`Java`
- 是否支持分片：支持
- 支持的数据库：MySQL/Mariadb、Oracle、DB2、SQL Server、PostgreSQL
- 路由规则：事务包裹的SQL会全部走写库、没有事务包裹SQL读写库通过设置Hint实现。其它功能通过配置文件实现。
- 简介：mycat 2013年从阿里cobar分离出来重构而成，至今还一直在更新。据官方文档介绍2015年就已经有电信、银行级别的客户在用。mycat也是四个中间件中支持数据库类型最多、功能最全的。不管你是否使用mycat， <a href="http://www.mycat.org.cn/document/mycat-definitive-guide.pdf" target="_blank" rel="noopener">mycat权威指南</a> 这个PDF文件建议大家都看一看，里面详细介绍了各种主从复制方法、分库/分表的规则、如何实现以及它们优缺点等等，作者2016年写这本书应该花费了很多时间与精力。2016年后mycat官方文档、wiki以及配套的mycat-web几乎停滞了，这也是mycat需要吐槽的地方。如果mycat能一直坚持更新完善文档以及配套的mycat-web，更合理有序的规划产品版本，那么mycat还真是第一选择。

### shardingsphere-proxy
- 官网：http://shardingsphere.apache.org/index_zh.html
- 开发语言：`Java`
- 是否支持分片：支持
- 支持的数据库：MySQL/Mariadb、PostgreSQL
- 路由规则：同一个线程且同一个数据库连接遇到有写操作那么之后的读操作都会读写库，同时也可以通过设置Hint强制读写库。其它功能通过配置文件实现。
- 简介：shardingsphere有三个产品，对于dotneter来说shardingsphere-proxy是唯一的选择。shardingsphere是当当网开源贡献给社区，京东在基础上发扬光大。已于2020年4月成为Apache基金会顶级项目，shardingsphere-proxy后期可以重点关注。网上搜索shardingsphere-proxy相关文档绝大部分都是copy了官方的介绍文档，相关案例文档也很少，可能还需要再养一养。

### proxysql
- 官网：https://proxysql.com/
- 开发语言：`C++`
- 是否支持分片：支持
- 支持的数据库：MySQL/Mariadb
- 简介：proxysql也是一款成熟的MySQL/Mariadb数据库中间件。官网文档完整，使用案例应该是4款中间件中最丰富和最多的。ProxySQL 的路由规则非常灵活，可以基于用户，基于schema，以及单个sql语句实现路由规则定制。同样也可以通过Hint与路由规则配合指定路由。proxysql也是一个非常不错的选择。

### maxscale
- 官网：https://mariadb.com/kb/en/maxscale/
- 开发语言：`C`
- 是否支持分片：不支持
- 支持的数据库：MySQL/Mariadb
- 路由规则：事务包裹的SQL会全部走写库、没有事务包裹SQL读写库通过设置Hint实现。其它功能通过配置文件实现。
- 简介：maridb开发的一个MySQL/Mariadb数据中间，已经非常成熟。官网文档非常完整，使用案例丰富。同时它提供了很多过滤器，如HintFilter；NamedServerFilter该过滤器可以设置指定表不需要读写分离，全部路由到写库；TopFilter该过滤器可以设置查询最慢的N条sql路由到指定读库；其他过滤器请查看官方文档。maxscale对于数据库集群高可用性提供的配置应该是4款中最丰富的。


&ensp; &ensp; 通过对4款中间件的简单介绍，我们发现他们都有自己路由规则，最配合丰富配置实现读写分离，而不是简单粗暴的分离。也都都提供了Hint的支持以及后端数据库监控。对于我们c#代码端要做的事情只需设置Sql的Hint，其它的交给中间件处理。
> Hint作为一种 SQL 补充语法，在关系型数据库中扮演着非常重要的角色。它允许用户通过相关的语法影响 SQL 的执行方式，对 SQL 进行特殊的优化。
> 简单来说就是SQL语句前加注解,如maxscale指定读写库的Hint：SELECT * from table1;
> -- maxscale route to master

## Adnc为什么选择了maxscale
- Adnc后端数据库是Mariadb,Maxscale与Mariadb属于同一家公司开发
- Maxscale官方文档非常完整、网上案例也很多、以及严谨的版本控制
- Maxscale对后端数据库集群高可用性配置是最丰富的
- Maxscale路由规则是我最喜欢的且提供了丰富Filter自定义路由
- Maxscale搭配Mariadb性能是4款中最好的(也可能因为不支持分库/分表)
- Maxscale不支持分库、分表，Adnc定位是一个微服务框架，天然已经按服务分了库。
分表就没有办法了，然而太过度设计也不是好方法，合适才是最好的。


## EFCore生成maxscale的Hint
读写分离必须要部署集群，基于maxscale中间件实现，还需要安装maxsale。

EFCore的TagWith是什么请参考<a href="https://docs.microsoft.com/en-us/ef/core/querying/tags">官方文档</a>。
<a href="https://github.com/AlphaYu/Adnc/blob/master/src/ServerApi/Adnc.Infr.EfCore/Repositories/BaseRepository.cs">点击查看完整源码</a>

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
                return dbSet.TagWith(EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER);
            return dbSet;
        }

        public virtual async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null, int? commandTimeout = null, CommandType? commandType = null, bool writeDb = false)
        {
            if (writeDb)
                //这个方法集成了dapper实现复杂查询,读操作路由到写库
                sql = string.Concat("/* ", EfCoreConsts.MAXSCALE_ROUTE_TO_MASTER, " */", sql);
            return await DbContext.Database.GetDbConnection().QueryAsync<TResult>(sql, param, null, commandTimeout, commandType);
        }
}
```
基于maxscale要写的代码就是上面这些，数据库连接字符串与直连数据库一样，端口改成maxscale的端口。

## EFCore生成mycat的Hint
再介绍一下mycat如何生成Hint
同样也必须要先部署好集群，基于mycat中间件实现，还需要安装mycat。

`EFCore`生成mycat的Hint稍微复杂一些，`EFCore`的`TagWith`方法生成的Hint是这这样的
```sql
-- #mycat:db_type=master
SELECT * FROM TABLE1
```
mycat要求是这样
```sql
/*#mycat:db_type=master*/
SELECT * FROM TABLE1
```
&ensp; &ensp; 我以`Pomelo.EntityFrameworkCore.MySql`为例，简单点说就是`EFCore`有一个`IQuerySqlGeneratorFactory`接口，`Pomelo`的`MySqlQuerySqlGeneratorFactory`类实现了这个接口，`Create()`方法负责创建具体的`QuerySqlGenerator`，这个类负责查询SQL的生成。<a href="https://github.com/AlphaYu/Adnc/blob/master/src/ServerApi/Adnc.Infr.EfCore/AdncMySqlQuerySqlGeneratorFactory.cs">点击查看完整源码</a>。

&ensp; &ensp; 我们需要做三件事情，
- 新建工厂类`AdncMySqlQuerySqlGeneratorFactory`继承`MySqlQuerySqlGeneratorFactory`并覆写`Create()`方法。代码如下
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
基于mycat要写的代码就是上面这些，数据库连接字符串与直连数据库一样，端口改成mycat的端口。

---
WELL DONE，记得 [star&&fork](https://github.com/alphayu/adnc)
全文完，[ADNC](https://aspdotnetcore.net)一个可以落地的.NET微服务/分布式开发框架。