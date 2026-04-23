## 前言
&ensp; &ensp; `ADNC`目前使用了Mariadb与Mongodb两种数据库，Mariadb用于存储业务数据，Mongodb用于存储登录日志、操作日志与异常日志(Nlog)。Mongodb的仓储只用到了很少的功能，本文不做介绍。需要重点介绍的是如何使用仓储操作Mariadb。

- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/" title="如何使用仓储(二)-工作单元">如何使用仓储(二)-工作单元</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/" title="如何使用仓储(三)-CodeFirst">如何使用仓储(三)-CodeFirst</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sql/" title="如何使用仓储(四)-撸SQL">如何使用仓储(四)-撸SQL</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/" title="如何使用仓储(五)-切换数据库类型">如何使用仓储(五)-切换数据库类型</a>

## 总体设计
仓储相关基础架构层
- `Adnc.Infra.Repository` 定义仓储接口、工作单元接口、实体基类 、常量 
- `Adnc.Infra.EfCore` 实现了仓储接口`IEfRepository`、`IEfBasicRepository`、`IUnitOfWork`，该接口通过EFCore处理增删改查以及事务操作。
- `Adnc.Infra.Dapper` 实现了仓储接口 `IAdoExecuterRepository`、`IAdoQuerierRepository`、`IAdoExecuterWithQuerierRepository` 该接口通过Dapper执行原生Sql处理增删改查操作。
- `Adnc.Infra.Mongo` 实现了仓储接口 `IMongoRepository` 该接口通过MongoClient处理增删改查操作。  

三个仓储实现层中`Adnc.Infra.EfCore`是核心，EFCore是`ADNC`的主力ORM，`Adnc.Infra.Dapper`只是辅助。

>实际项目开发过程中总有那么有一些操作(可能占比很低)需要通过原生SQL去做，比方说复杂查询、多表联合查询、大批量更新(每条记录更新的内容不同)、大批量插入、大批量删除、直接调用存储过程、直接调用视图等等，极少数情况下不管通过哪种ORM都不如直接拼SQL强，再或者在同一个服务中mysql是核心业务库，但是除了核心业务库，还还一个或多个其他库需要查询，这些库有可能是sqlserver或者oracle。这些情况单靠EFCore无法应对。

## 仓储注册
仓储在`Application`工程的`xxxApplicationDependencyRegistrar.cs` 文件注册
```csharp
public sealed class UsrApplicationDependencyRegistrar : AbstractApplicationDependencyRegistrar
{
    services.AddDapperRepositories();
    services.AddEfCoreContextWithRepositories();
    services.AddMongoContextWithRepositries();
    //注册其他服务
}
```

## 通过构造函数注入使用

> 通过EfCore仓储的AdoQuerier属性可以直接调用Dapper所有的查询方法,例如：_efRepo.AdoQuerier.QueryFirstAsync<Customer>(sql,params)

```csharp
//注入EFCore仓储
public CustomerAppService(IEfRepository<Customer> efRepo)
{
    _efRepo = efRepo;
}
//注入Mongo仓储
public CustomerAppService(IMongoRepository<LoginLog> mongoRepo)
{
    _mongoRepo = mongoRepo;
}
//单独注入Dapper仓储，非必要情况下不要单独注入。
public CustomerAppService(IAdoExecuterWithQuerierRepository dapperRepo)
{
    _dapperRepo = dapperRepo;
}
```

## EFCore仓储方法使用介绍
本文所有的单元测试用例都在`EfCoreRepositoryTests.cs`文件<br/>
https://github.com/AlphaYu/Adnc/blob/master/test/Adnc.UnitTest/EfCoreRepositoryTests.cs

### InsertAsync
#### 方法签名
```csharp
/// <summary>
/// 插入单个实体
/// </summary>
/// <param name="entity"><see cref="TEntity"/></param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestInsert()
{
    var id = IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId);
    var radmon = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    var cusotmer = new Customer
    {
        Id = id
        ,
        Account = $"a{radmon}"
        ,
        Realname = $"r{radmon}"
        ,
        Nickname = $"n{radmon}"
        ,
        FinanceInfo = new CustomerFinance { Id = id, Account = $"a{radmon}", Balance = 0 }
    };

    await _customerRsp.InsertAsync(cusotmer);
}
```
#### 生成的Sql
```sql
--EF生成的Sql
SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ
START TRANSACTION
INSERT INTO `Customer` (`Id`, `Account`, `CreateBy`, `CreateTime`, `ModifyBy`, `ModifyTime`, `Nickname`, `Realname`)
VALUES (156081863398658048, 'a1615020816095', 1600000000000, TIMESTAMP('2021-03-06 16:53:37.051070'), NULL, NULL, 'n1615020816095', 'r1615020816095')
INSERT INTO `CustomerFinance` (`Id`, `Account`, `Balance`, `CreateBy`, `CreateTime`, `ModifyBy`, `ModifyTime`)
VALUES (156081863398658048, 'a1615020816095', 0, 1600000000000, TIMESTAMP('2021-03-06 16:53:37.051179'), NULL, NULL);
COMMIT
```

### InsertRangeAsync
#### 方法签名
```csharp
/// <summary>
/// 批量插入实体
/// </summary>
/// <param name="entities"><see cref="TEntity"/></param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestInsertRange()
{
    var customer = await _customerRsp.FetchAsync(x => x.Id > 1);

    var logs = new List<CustomerTransactionLog>
    {
        new CustomerTransactionLog{ Id=IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId),Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeTypeEnum.Recharge,ExchageStatus=ExchageStatusEnum.Finished,Remark="test"}
        ,
        new CustomerTransactionLog{ Id=IdGenerater.GetNextId(IdGenerater.DatacenterId, IdGenerater.WorkerId),Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeTypeEnum.Recharge,ExchageStatus=ExchageStatusEnum.Finished,Remark="test"}
    };

    await _custLogsRsp.InsertRangeAsync(logs);
}
```
#### 生成的Sql
```sql
--EF生成的sql
INSERT INTO `CustomerTransactionLog` (`Id`, `Account`, `Amount`, `ChangedAmount`, `ChangingAmount`, `CreateBy`, `CreateTime`, `CustomerId`, `ExchageStatus`, `ExchangeType`, `Remark`)
VALUES 
(156054605791367168, 'a1615011019854', 0, 0, 0, 1600000000000, timestamp('2021-03-06 15:05:17.517754'), 156040774981652480, 2008, 8000, 'test'),
(156054605799755776, 'a1615011019854', 0, 0, 0, 1600000000000, timestamp('2021-03-06 15:05:17.517847'), 156040774981652480, 2008, 8000, 'test')
```

### UpdateAsync
不指定更新字段，实体必须是跟踪状态。如果不是会显示抛出异常，提示需要指定更新列。
#### 方法签名
```csharp
/// <summary>
/// 更新单个实体
/// </summary>
/// <param name="entity"><see cref="TEntity"/></param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestUpdateWithTraking()
{
    //IEfRepository<>默认关闭了跟踪，需要手动开启跟踪
    var customer = await _customerRsp.FetchAsync(x => x.Id > 1, noTracking: false);
    //实体已经被跟踪
    customer.Realname = "被跟踪";
    await _customerRsp.UpdateAsync(customer);
    var newCusts = await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE Id=@Id", customer);
    Assert.Equal("被跟踪", newCusts.FirstOrDefault().Realname);

    //实体已经被跟踪,主从表同时更新
    customer = await _customerRsp.FetchAsync(x => x.Id > 1, x => x.FinanceInfo, noTracking: false);
    customer.Account = "主从更新";
    customer.FinanceInfo.Account = "主从更新";
    await _customerRsp.UpdateAsync(customer);
    var newCust = await _customerRsp.FetchAsync(x => x.Id == customer.Id, x => x.FinanceInfo);
    Assert.Equal("主从更新", newCust.Account);
    Assert.Equal("主从更新", newCust.FinanceInfo.Account);
}
```
#### 生成的Sql
```sql
--EF生成的sql
UPDATE `Customer` SET `ModifyBy` = 1600000000000, `ModifyTime` = timestamp('2021-03-06 14:49:52.377305'), `Realname` = '被跟踪' WHERE `Id` = 156040774981652480;

SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ
START TRANSACTION
UPDATE `Customer` SET `Account` = '主从更新', `ModifyTime` = TIMESTAMP('2021-03-06 21:39:06.906343')
WHERE `Id` = 156151316735987712;
SELECT ROW_COUNT()
UPDATE `CustomerFinance` SET `Account` = '主从更新', `ModifyBy` = 1600000000000, `ModifyTime` = TIMESTAMP('2021-03-06 21:39:06.906354')
WHERE `Id` = 156151316735987712 AND `RowVersion` = TIMESTAMP('2021-03-06 21:29:35.274606');
SELECT `RowVersion`
FROM `CustomerFinance`
WHERE ROW_COUNT() = 1 AND `Id` = 156151316735987712
COMMIT
```

### UpdateAsync
指定更新字段，实体可以是任何状态。
#### 方法签名
```csharp
/// <summary>
/// 更新单个实体
/// </summary>
/// <param name="entity"><see cref="entity"/></param>
/// <param name="updatingExpressions">需要更新列的表达式树数组</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>>[] updatingExpressions, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async void TestUpdateAssigns()
{
    var customer = await _customerRsp.FindAsync(154951941552738304, noTracking: false);
    //实体已经被跟踪并且指定更新列
    customer.Nickname = "更新指定列";
    customer.Realname = "不指定该列";
    //更新列没有指定Realname，该列不会被更新
    await _customerRsp.UpdateAsync(customer, UpdatingProps<Customer>(c => c.Nickname));
    var newCus = (await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@ID", customer)).FirstOrDefault();
    Assert.Equal("更新指定列", newCus.Nickname);
    Assert.NotEqual("不指定该列", newCus.Realname);

    //实体没有被跟踪，dbcontext中有同名实体
    var id = customer.Id;
    await _customerRsp.UpdateAsync(new Customer { Id = id, Realname = "没被跟踪01", Nickname = "新昵称" }, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));
    newCus = (await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@ID", customer)).FirstOrDefault();
    Assert.Equal("没被跟踪01", newCus.Realname);
    Assert.Equal("新昵称", newCus.Nickname);


    //实体没有被跟踪，dbcontext中有没有同名实体
    id = 154959990543749120;
    await _customerRsp.UpdateAsync(new Customer { Id = id, Realname = "没被跟踪02", Nickname = "新昵称" }, UpdatingProps<Customer>(c => c.Realname, c => c.Nickname));
    newCus = await _customerRsp.FindAsync(id);
    Assert.Equal("没被跟踪02", newCus.Realname);
    Assert.Equal("新昵称", newCus.Nickname);
}
```
#### 生成的Sql
```sql
--EF生成的sql
UPDATE `Customer` SET `ModifyTime` = timestamp('2021-03-06 15:38:51.089013'), `Nickname` = '更新指定列'
WHERE `Id` = 154951941552738304;

UPDATE `Customer` SET `ModifyTime` = timestamp('2021-03-06 15:39:09.736015'), `Nickname` = '新昵称'
WHERE `Id` = 154951941552738304;

UPDATE `Customer` SET `ModifyBy` = 1600000000000, `ModifyTime` = timestamp('2021-03-06 15:39:21.223032'), `Nickname` = '新昵称', `Realname` = '没被跟踪02'
WHERE `Id` = 154959990543749120;
```

### UpdateRangeAsync
根据条件批量更新
#### 方法签名
```csharp
/// <summary>
/// 批量更新
/// </summary>
/// <param name="whereExpression">查询条件</param>
/// <param name="updatingExpression">需要更新的字段</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> UpdateRangeAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TEntity>> updatingExpression, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async void TestUpdateRange()
{
    var cus1 = await this.InsertCustomer();
    var cus2 = await this.InsertCustomer();
    var total = await _customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
    Assert.Equal(2, total);

    await _customerRsp.UpdateRangeAsync(c => c.Id == cus1.Id || c.Id == cus2.Id, x => new Customer { Realname = "批量更新" });
    var result2 = await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
    Assert.NotEmpty(result2);
    Assert.Equal("批量更新", result2.FirstOrDefault().Realname);
    Assert.Equal("批量更新", result2.LastOrDefault().Realname);
}
```
#### 生成的Sql
```sql
--EF生成的sql
UPDATE `Customer` AS A
INNER JOIN ( SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
FROM `Customer` AS `c`
WHERE (`c`.`Id` = 156193791336910848) OR (`c`.`Id` = 156193806604177408)
           ) AS B ON A.`Id` = B.`Id`
SET A.`Realname` = '批量更新'
```

###  DeleteAsync
根据Id删除
#### 方法签名
```csharp
/// <summary>
/// 删除实体
/// </summary>
/// <param name="keyValue">Id</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> DeleteAsync(long keyValue, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestDelete()
{
    //single hard delete 
    var customer = await this.InsertCustomer();
    var customerFromDb = await _customerRsp.FindAsync(customer.Id);
    Assert.Equal(customer.Id, customerFromDb.Id);

    await _customerRsp.DeleteAsync(customer.Id);
    var result = await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID=@Id", new { Id = customer.Id });
    Assert.Empty(result);
}
```
#### 生成的Sql
```sql
--EF生成的sql
DELETE FROM `Customer`
WHERE `Id` = 156040229583720448;
```

### DeleteRangeAsync 
根据条件批量删除
#### 方法签名
```csharp
/// <summary>
/// 批量删除实体
/// </summary>
/// <param name="whereExpression">查询条件</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> DeleteRangeAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
//测试批量删除
[Fact]
public async Task TestDeleteRange()
{
    //batch hand delete
    var cus1 = await this.InsertCustomer();
    var cus2 = await this.InsertCustomer();
    var total = await _customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
    Assert.Equal(2, total);

    await _customerRsp.DeleteRangeAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
    var result2 = await _customerRsp.QueryAsync<Customer>("SELECT * FROM Customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
    Assert.Empty(result2);
}
```
#### 生成的Sql
```sql
--EF生成的sql
DELETE A
FROM `Customer` AS A
INNER JOIN ( SELECT `c`.`Id`
FROM `Customer` AS `c`
WHERE (`c`.`Id` = 156163586249592832) OR (`c`.`Id` = 156163592918536192) ) AS B ON A.`Id` = B.`Id`
```

### FindAsync
根据主键Id查询
#### 方法签名
```csharp
/// <summary>
/// 根据Id查询，返回单个实体
/// </summary>
/// <param name="keyValue">Id</param>
/// <param name="navigationPropertyPath">导航属性，可选参数</param>
/// <param name="writeDb">是否读写库,默认false，可选参数</param>
/// <param name="noTracking">是否开启跟踪，默认不开启，可选参数</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns><see cref="TEntity"/></returns>
Task<TEntity> FindAsync(long keyValue, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestFind()
{
    //不加载导航属性
    var customer3 = await _customerRsp.FindAsync(154959990543749120);
    Assert.NotNull(customer3);
    Assert.Null(customer3.FinanceInfo);

    //加载导航属性
    var customer4 = await _customerRsp.FindAsync(154959990543749120, x => x.TransactionLogs);
    Assert.NotNull(customer4);
    Assert.NotEmpty(customer4.TransactionLogs);
}
```
#### 生成的Sql
```sql
--EF生成的sql
SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
FROM `Customer` AS `c`
WHERE `c`.`Id` = 154959990543749120
LIMIT 1

SELECT `t`.`Id`, `t`.`Account`, `t`.`CreateBy`, `t`.`CreateTime`, `t`.`ModifyBy`, `t`.`ModifyTime`, `t`.`Nickname`, `t`.`Realname`, `c0`.`Id`, `c0`.`Account`, `c0`.`Amount`, `c0`.`ChangedAmount`, `c0`.`ChangingAmount`, `c0`.`CreateBy`, `c0`.`CreateTime`, `c0`.`CustomerId`, `c0`.`ExchageStatus`, `c0`.`ExchangeType`, `c0`.`Remark`
FROM (
    SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
    FROM `Customer` AS `c`
    WHERE `c`.`Id` = 154959990543749120
    LIMIT 1
) AS `t`
LEFT JOIN `CustomerTransactionLog` AS `c0` ON `t`.`Id` = `c0`.`CustomerId`
ORDER BY `t`.`Id`, `c0`.`Id`
```

### FetchAsync
根据查询条件返回第一条
#### 方法签名
```csharp
/// <summary>
/// 根据条件查询,返回单个实体
/// </summary>
/// <param name="whereExpression">查询条件</param>
/// <param name="navigationPropertyPath">导航属性,可选参数</param>
/// <param name="orderByExpression">排序字段，默认主键，可选参数</param>
/// <param name="ascending">排序方式，默认逆序，可选参数</param>
/// <param name="writeDb">是否读写库,默认false，可选参数</param>
/// <param name="noTracking">是否开启跟踪，默认不开启，可选参数</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
Task<TEntity> FetchAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, dynamic>> navigationPropertyPath = null, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);

/// <summary>
/// 根据条件查询,返回单个实体或对象
/// </summary>
/// <typeparam name="TResult">匿名对象</typeparam>
/// <param name="selector">选择器</param>
/// <param name="whereExpression">查询条件</param>
/// <param name="orderByExpression">排序字段，默认主键，可选参数</param>
/// <param name="ascending">排序方式，默认逆序，可选参数</param>
/// <param name="writeDb">是否读写库,默认false，可选参数</param>
/// <param name="noTracking">是否开启跟踪，默认不开启，可选参数</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
Task<TResult> FetchAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, bool ascending = false, bool writeDb = false, bool noTracking = true, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestFetch()
{
    //指定列查询
    var customer = await _customerRsp.FetchAsync(x => new { x.Id, x.Account}, x => x.Id > 1);
    Assert.NotNull(customer);

    //指定列查询，指定列包含导航属性
    var customer2 = await _customerRsp.FetchAsync(x => new { x.Id, x.Account, x.FinanceInfo }, x => x.Id > 1);
    Assert.NotNull(customer2);

    //不指定列查询
    var customer3 = await _customerRsp.FetchAsync(x => x.Id > 1);
    Assert.NotNull(customer3);

    //不指定列查询，预加载导航属性
    var customer4 = await _customerRsp.FetchAsync(x => x.Id > 1, x => x.FinanceInfo);
    Assert.NotNull(customer4);
}
```
#### 生成的Sql
```sql
--EF生成的sql
SELECT `c`.`Id`, `c`.`Account`
FROM `Customer` AS `c`
WHERE `c`.`Id` > 1
ORDER BY `c`.`Id` DESC
LIMIT 1

SELECT `c`.`Id`, `c`.`Account`, `c0`.`Id`, `c0`.`Account`, `c0`.`Balance`, `c0`.`CreateBy`, `c0`.`CreateTime`, `c0`.`ModifyBy`, `c0`.`ModifyTime`, `c0`.`RowVersion`
FROM `Customer` AS `c`
LEFT JOIN `CustomerFinance` AS `c0` ON `c`.`Id` = `c0`.`Id`
WHERE `c`.`Id` > 1
ORDER BY `c`.`Id` DESC
LIMIT 1

SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
FROM `Customer` AS `c`
WHERE `c`.`Id` > 1
ORDER BY `c`.`Id` DESC
LIMIT 1

SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`, `c0`.`Id`, `c0`.`Account`, `c0`.`Balance`, `c0`.`CreateBy`, `c0`.`CreateTime`, `c0`.`ModifyBy`, `c0`.`ModifyTime`, `c0`.`RowVersion`
FROM `Customer` AS `c`
LEFT JOIN `CustomerFinance` AS `c0` ON `c`.`Id` = `c0`.`Id`
WHERE `c`.`Id` > 1
ORDER BY `c`.`Id` DESC
LIMIT 1

```

### AnyAsync 
查询实体是否已经存在，调用Ef的原生方法。
#### 方法签名
```csharp
/// <summary>
/// 根据条件查询实体是否存在
/// </summary>
/// <param name="whereExpression">查询条件</param>
/// <param name="writeDb">是否读写库，默认false,可选参数</param>
/// param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);
```

### CountAsync 
统计符合条件的实体数量，调用Ef的原生方法。
#### 方法签名
```csharp
/// <summary>
/// 统计符合条件的实体数量
/// </summary>
/// <param name="whereExpression">查询条件</param>
/// <param name="writeDb">是否读写库，默认false,可选参数</param>
/// param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression, bool writeDb = false, CancellationToken cancellationToken = default);
```
### Where,GetAll 
这是两个万能查询方法，根据查询条件返回一个IQueryable。
`GetAll()` `==` `Where(x=>true)`
#### 方法签名
```csharp
/// <summary>
/// 根据条件查询，返回IQueryable<TEntity>
/// </summary>
/// <param name="expression">查询条件</param>
/// <param name="writeDb">是否读写库，默认false,可选参数</param>
/// <param name="noTracking">是否开启跟踪，默认false,可选参数</param>
IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression, bool writeDb = false, bool noTracking = true);

 /// <summary>
 /// 返回IQueryable<TEntity>
 /// </summary>
 /// <param name="writeDb">是否读写库，默认false,可选参数</param>
 /// <param name="noTracking">是否开启跟踪，默认false,可选参数</param>
IQueryable<TEntity> GetAll(bool writeDb = false, bool noTracking = true);
```
#### 单元测试
```csharp
//测试查询
[Fact]
public async Task TestWhereAndGetAll()
{
    //返回集合
    var customers = await _customerRsp.Where(x => x.Id > 1).ToListAsync();
    Assert.NotEmpty(customers);

    //返回单个
    var customer = await _customerRsp.Where(x => x.Id > 1).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
    Assert.NotNull(customer);

    //组合查询
    //GetAll() = Where(x=>true)
    var customerAll = _customerRsp.GetAll();
    var custsLogs = _custLogsRsp.GetAll();

    var logs = await customerAll.Join(custsLogs, c => c.Id, t => t.CustomerId, (c, t) => new
    {
        t.Id
        ,
        t.CustomerId
        ,
        t.Account
        ,
        t.ChangedAmount
        ,
        t.ChangingAmount
        ,
        c.Realname
    })
    .Where(c => c.Id > 1)
    .ToListAsync();

    Assert.NotEmpty(logs);
}
```
#### 生成的Sql
```sql
--EF生成的sql
SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
FROM `Customer` AS `c`
WHERE `c`.`Id` > 1

SELECT `c`.`Id`, `c`.`Account`, `c`.`CreateBy`, `c`.`CreateTime`, `c`.`ModifyBy`, `c`.`ModifyTime`, `c`.`Nickname`, `c`.`Realname`
FROM `Customer` AS `c`
WHERE `c`.`Id` > 1
ORDER BY `c`.`Id` DESC
LIMIT 1

SELECT `c0`.`Id`, `c0`.`CustomerId`, `c0`.`Account`, `c0`.`ChangedAmount`, `c0`.`ChangingAmount`, `c`.`Realname`
FROM `Customer` AS `c`
INNER JOIN `CustomerTransactionLog` AS `c0` ON `c`.`Id` = `c0`.`CustomerId`
WHERE `c0`.`Id` > 1
```
---

WELL DONE
全文完