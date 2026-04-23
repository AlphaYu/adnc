## 前言
&ensp; &ensp; 本文主要介绍在`ADNC`仓储中如何撸SQL。当遇到复杂查询、多表查询、大批量写操作或者其他原因时，勉强使用EFCore原生方法去做，不一定是好的实现。
国产ORM中的优秀者`SqlSugar`,`FreeSql`都提供直接操作ADO执行SQL的功能，也就是说生产环境中撸SQL是避免不了的。

- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/" title="如何使用仓储(二)-工作单元">如何使用仓储(二)-工作单元</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/" title="如何使用仓储(三)-CodeFirst">如何使用仓储(三)-CodeFirst</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sql/" title="如何使用仓储(四)-撸SQL">如何使用仓储(四)-撸SQL</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/" title="如何使用仓储(五)-切换数据库类型">如何使用仓储(五)-切换数据库类型</a>

## EFCore仓储中撸SQL
我们来看下EFCore仓储接口中与SQL有关的二个方法和一个属性，代码片段如下：
```csharp
public interface IEfRepository<TEntity> : IEfBaseRepository<TEntity>
where TEntity : EfEntity
{
    /// <summary>
    /// 执行原生Sql查询
    /// </summary>
    IAdoQuerierRepository? AdoQuerier { get; }

    /// <summary>
    /// 执行原生Sql写操作
    /// </summary>
    Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default);

    /// <summary>
    /// 执行原生Sql写操作
    /// </summary>
    Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);
}
```
再看下如何实现的
- `AdoQuerier`   
实现类在构造函数注入IAdoQuerierRepository接口，设置数据库连接后再赋值给该对象，ADNC中实现了该接口的只有adnc.infra.dapper工程中DapperRepository类。我们可以去看下IAdoQuerierRepository的定义，几乎包含了Dapper所有查询方法。也就是说我们在EFCore仓储中可以通过AdoQuerier属性调用Dapper所有的查询方法。
- `ExecuteSqlInterpolatedAsync`
直接调用EF原生方法DbContext.Database.ExecuteSqlInterpolatedAsync()执行Sql(写操作)，该方法可以避免SQL注入的问题，尽量用该方法。
- `ExecuteSqlRawAsync`
直接调用EF原生方法DbContext.Database.ExecuteSqlRawAsync()执行Sql(写操作)。
```csharp
public sealed class EfRepository<TEntity> : AbstractEfBaseRepository<AdncDbContext, TEntity>, IEfRepository<TEntity>
    where TEntity : EfEntity, new()
{
    private readonly IAdoQuerierRepository _adoQuerier;

    public EfRepository(AdncDbContext dbContext, IAdoQuerierRepository adoQuerier = null)
        : base(dbContext)
    => _adoQuerier = adoQuerier;

    public IAdoQuerierRepository AdoQuerier
    {
        get
        {
            if (_adoQuerier is null)
                return null;
            if (!_adoQuerier.HasDbConnection())
                _adoQuerier.ChangeOrSetDbConnection(DbContext.Database.GetDbConnection());
            return _adoQuerier;
        }
    }

    public async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql, CancellationToken cancellationToken = default)
        => await DbContext.Database.ExecuteSqlInterpolatedAsync(sql, cancellationToken);

    public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
        => await DbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
}
```
部分示例代码

```csharp
var sql2 = @"SELECT * FROM Customer ORDER BY Id ASC";
var dbCustomer = await _customerRsp.AdoQuerier.QueryFirstAsync<Customer>(sql2, null, _customerRsp.CurrentDbTransaction);

var rawSql1 = "update Customer set nickname='test8888' where id=1000000000";
var rows = await _customerRsp.ExecuteSqlRawAsync(rawSql1);

var id=10000000;
var newNickName = "test8888";
FormattableString formatSql2 = $"update Customer set nickname={newNickName} where id={id}";
rows = await _customerRsp.ExecuteSqlInterpolatedAsync(formatSql2);
```
## 直接使用Dapper仓储
> 除非没有办法了，否则尽量不要直接使用Dapper仓储。当然直接使用Dapper仓储可以操作它支持的任意数据库，非常灵活。

```csharp
public class xxxAppService
{
    private IAdoExecuterWithQuerierRepository _dapperRepo;
    public xxxAppService(IAdoExecuterWithQuerierRepository dapperRepo)
    {
        _dapperRepo = dapperRepo;
        _dapperRepo.ChangeOrSetDbConnection(connectingstring,DbTypes.MYSQL);
    }

    public Demomethod()
    {
        var sql="SELECT * FROM Customer Order by Id desc";
        var result = await _dapperRepo.QueryAsync(sql);
    }
}
```

WELL DONE
全文完