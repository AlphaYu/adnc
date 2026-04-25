## ADNC 如何使用仓储 - 基础功能
[GitHub 仓库地址](https://github.com/alphayu/adnc)

本文重点介绍如何使用仓储操作 MariaDB/Mysql。

## 总体设计
仓储相关基础架构层
- `Adnc.Infra.Repository` 定义仓储接口、工作单元接口、实体基类与常量。
- `Adnc.Infra.Repository.EfCore` 实现仓储接口 `IEfRepository`、`IEfBasicRepository`、`IUnitOfWork`，通过 EF Core 完成增删改查及事务操作。
- `Adnc.Infra.Repository.Dapper` 实现仓储接口 `IAdoExecuterRepository`、`IAdoQuerierRepository`、`IAdoExecuterWithQuerierRepository`，通过 Dapper 执行原生 SQL 以完成增删改查操作。

三个仓储实现层中，`Adnc.Infra.Repository.EfCore` 是核心：EF Core 是 `ADNC` 的主力 ORM，而 `Adnc.Infra.Dapper` 作为辅助能力补充。

## 通过构造函数注入使用

```csharp
//注入EFCore仓储
public CustomerAppService(IEfRepository<Customer> efRepo)
{
    _efRepo = efRepo;
}

//单独注入Dapper仓储，非必要情况下不要单独注入。
public CustomerAppService(IAdoExecuterWithQuerierRepository dapperRepo)
{
    _dapperRepo = dapperRepo;
}
```

## EFCore仓储方法使用介绍
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
public async Task TestInsertSingle01()
{
    var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();
    var customter = GenerateCustomer();
    var id = customter.Id;
    await customerRsp.InsertAsync(customter);

    var newCust = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT *  FROM customer WHERE Id=@Id", new { Id = id });
    Assert.NotEmpty(newCust);
}
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
    var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();
    var custLogsRsp = fixture.Container.GetRequiredService<IEfRepository<CustomerTransactionLog>>();

    var customer = await customerRsp.FetchAsync(x => x.Id > 1);

    var id0 = UnittestHelper.GetNextId();
    var id1 = UnittestHelper.GetNextId();
    var logs = new List<CustomerTransactionLog>
    {
        new CustomerTransactionLog{ Id=id0,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
        ,
        new CustomerTransactionLog{ Id=id1,Account=customer.Account,ChangedAmount=0,Amount=0,ChangingAmount=0,CustomerId=customer.Id,ExchangeType=ExchangeBehavior.Recharge,ExchageStatus=ExchageStatus.Finished,Remark="test"}
    };

    await custLogsRsp.InsertRangeAsync(logs);

    var logsFromDb = await custLogsRsp.Where(x => x.Id == id0 || x.Id == id1).ToListAsync();
    Assert.NotEmpty(logsFromDb);
    Assert.Equal(2, logsFromDb.Count);
}
```
### UpdateAsync
不指定更新字段，实体必须是跟踪状。
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
    var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

    var cus1 = await InsertCustomer();
    var cus2 = await InsertCustomer();

    var ids = await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM customer where ID in @ids ORDER BY ID", new { ids = new[] { cus1.Id, cus2.Id } });
    var id0 = ids.ToArray()[0];

    // IEfRepository<> disables tracking by default, so tracking must be enabled manually.
    var customer = await customerRsp.FetchAsync(x => x.Id == id0, noTracking: false);
    // The entity is already tracked.
    customer.Realname = "Tracked01";
    await customerRsp.UpdateAsync(customer);
    var newCust1 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE Id=@Id", new { Id = id0 });
    Assert.Equal("Tracked01", newCust1.FirstOrDefault().Realname);

    var customerId = (await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id  FROM customerfinance limit 0,1")).FirstOrDefault();
    customer = await customerRsp.FetchAsync(x => x.Id == customerId, x => x.FinanceInfo, noTracking: false);
    customer.Account = "ParentChildUpdate01";
    customer.FinanceInfo.Account = "ParentChildUpdate01";
    await customerRsp.UpdateAsync(customer);
    var newCust2 = await customerRsp.FetchAsync(x => x.Id == customerId, x => x.FinanceInfo);
    Assert.Equal("ParentChildUpdate01", newCust2.Account);
    Assert.Equal("ParentChildUpdate01", newCust2.FinanceInfo.Account);
}
```
### ExecuteUpdateAsync
指定更新字段，实体可以是任何状态。
#### 方法签名
```csharp
/// <summary>
/// Batch update.
/// </summary>
/// <param name="whereExpression">Query condition</param>
/// <param name="setPropertyCalls">Fields to update</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> ExecuteUpdateAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestUpdateAssigns()
{
    var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

    var cus1 = await InsertCustomer();
    var cus2 = await InsertCustomer();
    var cus3 = await InsertCustomer();
    var cus4 = await InsertCustomer();

    var ids = await customerRsp.AdoQuerier.QueryAsync<long>("SELECT Id FROM customer where ID in @ids ORDER BY ID", new { ids = new[] { cus1.Id, cus2.Id, cus3.Id, cus4.Id } });
    var id0 = ids.ToArray()[0];
    var customer = await customerRsp.FetchAsync(x=>x.Id == id0, noTracking: false);
    // The entity is already tracked and specific columns are selected, so Realname will not be updated.
    customer.Nickname = "UpdateSelectedColumn";
    customer.Realname = "ColumnNotSelected";
    await customerRsp.ExecuteUpdateAsync(x => x.Id == customer.Id,
                                         setters => setters.SetProperty(c => c.Nickname, "UpdateSelectedColumn"));
    var newCus = (await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID=@ID", customer)).FirstOrDefault();
    Assert.Equal("UpdateSelectedColumn", newCus.Nickname);
    Assert.NotEqual("ColumnNotSelected", newCus.Realname);
}
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
### ExecuteDeleteAsync
根据条件批量删除
#### 方法签名
```csharp
/// <summary>
/// Batch delete entities.
/// </summary>
/// <param name="whereExpression">Query condition</param>
/// <param name="cancellationToken"><see cref="CancellationToken"/></param>
/// <returns></returns>
Task<int> ExecuteDeleteAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken cancellationToken = default);
```
#### 单元测试
```csharp
[Fact]
public async Task TestDeleteRange()
{
    var customerRsp = fixture.Container.GetRequiredService<IEfRepository<Customer>>();

    //batch hand delete
    var cus1 = await InsertCustomer();
    var cus2 = await InsertCustomer();
    var total = await customerRsp.CountAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
    Assert.Equal(2, total);

    await customerRsp.ExecuteDeleteAsync(c => c.Id == cus1.Id || c.Id == cus2.Id);
    var result2 = await customerRsp.AdoQuerier.QueryAsync<Customer>("SELECT * FROM customer WHERE ID in @ids", new { ids = new[] { cus1.Id, cus2.Id } });
    Assert.Equal(0, result2.Count());
}
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
---

—— 完 ——
