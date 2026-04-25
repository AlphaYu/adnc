## ADNC 如何使用仓储 - 事务
[GitHub 仓库地址](https://github.com/alphayu/adnc)

本文主要介绍事务与工作单元，[EFCore的事务](https://docs.microsoft.com/zh-cn/ef/core/saving/transactions)分为以下三种

- SaveChanges
- DbContextTransaction
- TransactionScope

EF Core 默认情况下，SaveChanges 会根据需要开启事务。
1. `ExecuteUpdateAsync`、`ExecuteDeleteAsync` 等批量更新/删除不走 EF Core 原生变更跟踪流程，不受 SaveChanges 事务控制。
2. CAP 的事务也不受 SaveChanges 事务控制。
3. 原生 SQL 的增删改查不受 SaveChanges 事务控制。
4. 与读写分离相关的实现约束（请参考《如何实现读写分离》）。

```csharp
public class AdncDbContext : DbContext
{
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //efcore7 support this feature , default is WhenNeeded
        //Database.AutoTransactionBehavior = AutoTransactionBehavior.WhenNeeded;
        SetAuditFields();
        var result = base.SaveChangesAsync(cancellationToken);
        return result;
    }
}
```

`ADNC` 通过 `DbContextTransaction` 统一控制事务。如果业务逻辑中需要多次执行增/删/改操作（例如 ≥ 2 次），则建议显式开启事务控制（例如通过拦截器/特性声明）。

> 主从表插入、主从表更新、批量新增、批量修改、批量删除操作都可以通过一个增/删/改的方法实现，并不需要显示开启事务。

`Adnc.Infra.Repository.EfCore.MySql`实现了`Adnc.Infra.Repository`的`IUnitOfWork`接口，我们只需要显示声明拦截器特性或者从构造函数注入。

## 如何使用
### 注册拦截器
`services.AddAppliactionSerivcesWithInterceptors()`该扩展方法中以及实现好，具体实现请参考源代码。

### 显式声明拦截器
统一在`Adnc.Xxx.Application.Contracts`层服务接口声明也就是在接口上声明。
```csharp
//本地事务
[UnitOfWork]
Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input);

//CAP事务/分布式事务
[UnitOfWork(Distributed =true)]
Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount);
```
---
### 自由调用开启事务
如果你不喜欢拦截器处理事务或者拦截器处理事务不能满足需求，那么你也可以自由开启。
```csharp
public class xxxAppService
{
    private IUnitOfWork _uow;
    public xxxAppService(IUnitOfWork uow) _uow=>uow;
    public DemoMethod()
    {
        try
        {
            _uow.BeginTransaction(distributed:false);
            //操作1
            //操作2
            //操作3
            //操作N
            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
        }
        finally
        {
            _unitOfWork.Dispose();
        }  
    }
}

```
------
—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
