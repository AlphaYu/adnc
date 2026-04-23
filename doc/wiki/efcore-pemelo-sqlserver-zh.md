## 前言
&ensp; &ensp; 本文主要介绍在`ADNC`仓储中如何切换数据库类型，只要是EFCore支持的数据库类型ADNC都支持顺滑切换。ADNC默认使用的数据库类型是mariadb/mysql，本文将介绍如何从默认数据库类型切换到SqlServer，切换其他类型步骤都一样。数据库类型切换有两种场景：一是全局切换(所有服务的数据库类型都切换到另外一种类型，如Mysql切换到SqlServer)；二是部分切换(A服务使用MySql，B服务使用SqlServer，C服务使用Oracle），ADNC支持每个服务使用各自不同类型的数据库；。

- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-grud/" title="如何使用仓储(一)-基础功能">如何使用仓储(一)-基础功能</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/" title="如何使用仓储(二)-工作单元">如何使用仓储(二)-工作单元</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/" title="如何使用仓储(三)-CodeFirst">如何使用仓储(三)-CodeFirst</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sql/" title="如何使用仓储(四)-撸SQL">如何使用仓储(四)-撸SQL</a>
- <a href="https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/" title="如何使用仓储(五)-切换数据库类型">如何使用仓储(五)-切换数据库类型</a>

## 全局切换
全局切换需要调整4个文件，以下示例为将默认的MySql切换到SqlServer。

- `AbstractApplicationDependencyRegistrar.Repositories.cs`

```csharp
/*project:Adnc.Shared.Application*/
namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar 
{
	/// <summary>
    /// 注册EFCoreContext
    /// </summary>
    protected virtual void AddEfCoreContext()
    {
        //首选需要引用Adnc.Infra.Repository.EfCore.SqlServer工程
 		var sqlserverSection = Configuration.GetSection("SqlServer");
        Services.AddAdncInfraEfCoreSQLServer(sqlserverSection);
    }    
}
```
- `AbstractApplicationDependencyRegistrar.EventBus.cs`

  > option.UseMysql => option.UseSqlServer

```csharp
/*project:Adnc.Shared.Application*/
namespace Adnc.Shared.Application.Registrar;

public abstract partial class AbstractApplicationDependencyRegistrar
{
    /// <summary>
    /// 注册CAP组件(实现事件总线及最终一致性（分布式事务）的一个开源的组件)
    /// </summary>
    protected virtual void AddCapEventBus<TSubscriber>(
        Action<CapOptions> replaceDbAction = null,
        Action<CapOptions> replaceMqAction = null)
        where TSubscriber : class, ICapSubscribe
    {
        //other code
        Services.AddAdncInfraCap<TSubscriber>(option =>
        {
            if (replaceDbAction is not null)
            {
                replaceDbAction.Invoke(option);
            }
            else
            {
                var sqlsection = Configuration.GetSection("SqlServer");
                var connectionString = sqlsection.GetValue<string>("ConnectionString");
                //需要引用DotNetCore.CAP.SqlServer
                option.UseSqlServer(config =>
                {
                    config.ConnectionString = connectionString;
                    config.Schema = "cap";
                });
            }
        });
        //other code
    }                                      
}
```
- `AbstractWebApiDependencyRegistrar.HealthChecks.cs`

  > checkingMysql 调整为 checkingSqlserver

```csharp
/*project:Adnc.Shared.WebApi*/
namespace Adnc.Shared.WebApi.Registrar;

public abstract partial class AbstractWebApiDependencyRegistrar
{
    /// <summary>
    /// 注册健康监测组件
    /// </summary>
    protected IHealthChecksBuilder AddHealthChecks(
        bool checkingSqlserver = true,
        bool checkingMongodb = true,
        bool checkingRedis = true,
        bool checkingRabitmq = true)
    {
        //other code
        if (checkingSqlserver)
        {
            var sqlserverSection = Configuration.GetSection("SqlServer");
            if (sqlserverSection is null)
                throw new NullReferenceException("sqlserverSection is null");
            var connectionString = sqlserverSection.GetValue<string>("ConnectionString");
            //需要引用HealthChecks.SqlServer
            checksBuilder.AddSqlServer(connectionString);
        }
        //other code
    }
} 
```

- `appsettings.Development.json`

  > 删除MySql节点，新增SqlServer节点。

```json
  "SqlServer": {
    "ConnectionString": "Data Source=114.132.157.111;Initial Catalog=adnc_xxx_dev;User Id=sa;Password=xxx;"
  },
```

## 部分切换

部分切换相对简单很多，只需要覆写或者传递委托即可。下面将以whse服务为例。

1、`Adnc.Whse.Application`工程引用`Adnc.Infra.Repository.EfCore.SqlServer.csproj`

```csharp
namespace Adnc.Whse.Application.Registrar;

public sealed class WhseApplicationDependencyRegistrar : AbstractApplicationDepe
{
    public override void AddAdnc()
    {
        //other code
        //如果使用默认的mysql不需要传递replaceDbAction委托
      	AddCapEventBus<CapEventSubscriber>(replaceDbAction: capOption =>
        {
            var connectionString = _sqlSection.GetValue<string>("ConnectionString");
            capOption.UseSqlServer(config =>
            {
                config.ConnectionString = connectionString;
                config.Schema = "cap";
            });
        });
        //other code
    }
    
    //如果使用默认的mysql，不需要覆写该方法。
    protected override void AddEfCoreContext()
    {
      Services.AddAdncInfraEfCoreSQLServer(_sqlSection);
    }
}
```



2、`Adnc.Whse.Migrations`工程引用`Adnc.Infra.Repository.EfCore.SqlServer.csproj`并删除`Adnc.Infra.Repository.EfCore.Mysql.csproj`工程。

3、注册SqlServer健康检测，`WhseWebApiDependencyRegistrar.cs`

```csharp
namespace Adnc.Whse.WebApi.Registrar;

public sealed class WhseWebApiDependencyRegistrar : AbstractWebApiDependencyRegiter
{
    public override void AddAdnc()
    {
        AddWebApiDefault();
       //如果使用默认的Mysql,只需要AddHealthChecks(true, true, true, true)即可。
        var connectionString = Configuration.GetValue<string>("SqlServer:ConnectionString");
        AddHealthChecks(false, true, true, true).AddSqlServer(connectionString);

        Services.AddGrpc();
    }
}
```

- `appsettings.Development.json`

  > 删除MySql节点，新增SqlServer节点。

```json
  "SqlServer": {
    "ConnectionString": "Data Source=114.132.157.111;Initial Catalog=adnc_xxx_dev;User Id=sa;Password=xxx;"
  },
```
---
WELL DONE
全文完