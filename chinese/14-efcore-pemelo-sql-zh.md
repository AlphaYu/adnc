## ADNC 如何使用仓储 - 执行原生SQL

[GitHub 仓库地址](https://github.com/alphayu/adnc)
本文主要介绍在 `ADNC` 仓储中如何执行原生 SQL。当遇到复杂查询、多表查询、大批量写操作等场景时，勉强使用 EF Core 实现并不是最佳方案。例如 `SqlSugar`、`FreeSql`也提供直接操作 ADO 执行 SQL 的能力；因此在生产环境中，适当使用原生 SQL 往往不可避免。

## 在 EF Core 仓储中执行原生 SQL
下面以 EF Core 仓储接口为例，说明与 SQL 相关的一个属性与两个方法：
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
实现说明如下：
- `AdoQuerier`  
  实现类通过构造函数注入 `IAdoQuerierRepository` 接口，并在设置数据库连接后赋值给该对象。ADNC 中该接口的实现位于 `Adnc.Infra.Repository.Dapper` 工程的 `DapperRepository`。`IAdoQuerierRepository` 的定义几乎覆盖了 Dapper 的常用查询方法，因此可在 EF Core 仓储中通过 `AdoQuerier` 调用 Dapper 的查询能力。
- `ExecuteSqlInterpolatedAsync`  
  直接调用 EF 原生方法 `DbContext.Database.ExecuteSqlInterpolatedAsync()` 执行 SQL（写操作）。该方法可降低 SQL 注入风险，建议优先使用。
- `ExecuteSqlRawAsync`  
  直接调用 EF 原生方法 `DbContext.Database.ExecuteSqlRawAsync()` 执行 SQL（写操作）。

示例代码如下：

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
## 直接使用 Dapper 仓储
> 除非确有必要，否则不建议直接使用 Dapper 仓储。优先通过 EF Core 仓储的 `AdoQuerier` 统一访问，以降低数据访问层的耦合度并保持调用方式一致。直接使用 Dapper 仓储的优势在于可灵活操作其支持的任意数据库类型。

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

—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
