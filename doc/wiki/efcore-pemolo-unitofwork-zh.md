## 前言
&ensp; &ensp; <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a> 介绍了仓储的总体架构与设计思路以及仓储基础功能的使用。本文主要介绍事务与工作单元。
`Adnc.Infr.EfCore`是EFCore仓储与工作单元的实现工程，集成了EFcore作为仓储的实现。

- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-unitofwork" title="如何使用仓储(二)-工作单元">如何使用仓储(二)-分布式事务/本地事务</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-codefirst" title="如何使用仓储(三)-CodeFirst">如何使用仓储(三)-CodeFirst</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sql" title="如何使用仓储(四)-撸SQL">如何使用仓储(四)-撸SQL</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver" title="如何使用仓储(五)-切换数据库类型">如何使用仓储(五)-切换数据库类型</a>

## EFCore的事务
[EFCore的事务](https://docs.microsoft.com/zh-cn/ef/core/saving/transactions)分为以下三种
- SaveChanges
- DbContextTransaction
- TransactionScope

EFCore默认情况下，SaveChanges是开启了事务的。然而基于以下原因`ADNC`关闭了SaveChanges自动事务。
1. UpdateRangeAsync,DeleteRangeAsync 批量删除，批量更新方法不是走EfCore的原生方法，不受SaveChanges事务控制。
2. Cap的事务也不受SaveChanges事务控制
2. 原生SQL增删改查不受SaveChanges事务控制
4. 最后一点就是与读写分离相关，请参考`如何实现读写分离`

```csharp
public class AdncDbContext : DbContext
{
     public AdncDbContext()
     {
         Database.AutoTransactionsEnabled = false;
     }
}
```

在关闭SaveChanges自动事务后，`ADNC`通过DbContextTransaction统一控制事务。如果你的业务逻辑调用增/删/改的方法>=2次，就需要显示声明拦截器来开启事务控制。

> 主从表插入、主从表更新、批量新增、批量修改、批量删除操作都可以通过一个增/删/改的方法实现，并不需要显示开启事务。

`Adnc.Infr.EfCore.MySQL`实现了`Adnc.Infra.Repository`的`IUnitOfWork`接口，通过`servers.AddAdncInfraEfCoreMySql()`统一注册了该接口。工作单元拦截器在`Adnc.Application.Shared` 工程的 `Interceptors`目录中，都已经实现好。我们只需要在使用的地方注入拦截器，然后显示声明。

## 如何使用
### 注册拦截器
`services.AddAppliactionSerivcesWithInterceptors()`该扩展方法中以及实现好，具体实现请参考源代码。

> 如果采用经典三层开发模式，在Application或者Repository层注册都可以。
> 如果采用DDD开发模式，则只能在Application层注册。

在经典三层开发模式中，`ADNC`是在Application层注册的。~~Adnc是在Repository层注册的，主要还是基于锁定更少的sql以及灵活控制读写分离的目的。如果在Repository层使用工作单元来控制事务，意味着你需要多写一些一点代码。~~ 对于经典三层开发模式来说，在Application或者Repository层注册，都没有问题，取决于你自己的考虑。

```csharp
protected virtual void AddAppliactionSerivcesWithInterceptor(Action<IServiceCollection> action = null)
{
            Services.Add(new ServiceDescriptor(implType, implType, lifetime));

            var serviceDescriptor = new ServiceDescriptor(serviceType, provider =>
            {
                var interceptors = DefaultInterceptorTypes.ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToArray();
                var target = provider.GetService(implType);
                var interfaceToProxy = serviceType;
                var proxy = CastleProxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, target, interceptors);
                return proxy;
            }, lifetime);
            Services.Add(serviceDescriptor);
}
```

### 显示声明开启
统一在`Adnc.Xxx.Application.Contracts`层服务接口声明也就是在接口上声明。
```csharp
//本地事务
//如:https://github.com/AlphaYu/Adnc/blob/master/src/ServerApi/Services/Adnc.Usr/Adnc.Usr.Application.Contracts/Services/IRoleAppService.cs
[UnitOfWork]
Task<AppSrvResult> UpdateAsync(long id, DeptUpdationDto input);

//CAP事务/分布式事务
//如：https://github.com/AlphaYu/Adnc/blob/master/src/ServerApi/Services/Adnc.Cus/Adnc.Cus.Application.Contracts/Services/ICustomerAppService.cs
[UnitOfWork(SharedToCap = true)]
Task<AppSrvResult> ProcessPayingAsync(long transactionLogId, long customerId, decimal amount);
```
---
### 自由调用开启
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
WELL DONE，记得 [star&&fork](https://github.com/alphayu/adnc)
全文完，[ADNC](https://aspdotnetcore.net)一个可以落地的.NET微服务/分布式开发框架。