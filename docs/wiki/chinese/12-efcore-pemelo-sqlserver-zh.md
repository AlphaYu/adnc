## ADNC 如何使用仓储 - 切换数据库类型

[GitHub 仓库地址](https://github.com/alphayu/adnc)
本文主要介绍在 `ADNC` 仓储中如何切换数据库类型。原则上，只要 EF Core 支持的数据库类型，ADNC 均可实现平滑切换。ADNC 默认使用 MariaDB/MySQL，本文以从默认数据库类型切换到 SQL Server 为例；切换到其他数据库类型的步骤类似。
数据库类型切换通常有两类场景：
1) 全局切换：所有服务的数据库类型统一切换到另一种类型（例如 MySQL 切换到 SQL Server）。
2) 部分切换：不同服务使用不同数据库类型（例如服务 A 使用 MySQL，服务 B 使用 SQL Server，服务 C 使用 Oracle）。ADNC 支持各服务使用不同类型的数据库。

## 全局切换
全局切换需要调整3个文件，以下示例为将默认的MySql切换到SqlServer。

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
        AddOperater(_services);

        var connectionString = _configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var migrationsAssemblyName = _serviceInfo.MigrationsAssemblyName;
        _services.AddAdncInfraEfCoreSQLServer(RepositoryOrDomainLayerAssembly, optionsBuilder =>
        {
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(migrationsAssemblyName)
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }, _lifetime);
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
    protected virtual void AddCapEventBus(IEnumerable<Type> subscribers, Action<FailedInfo>? failedThresholdCallback = null)
    {
        ArgumentNullException.ThrowIfNull(subscribers, nameof(subscribers));
        Checker.Argument.ThrowIfNullOrCountLEZero(subscribers, nameof(subscribers));

        var connectionString = Configuration.GetValue<string>(NodeConsts.SqlServer_ConnectionString) ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var rabbitMQOptions = Configuration.GetRequiredSection(NodeConsts.RabbitMq).Get<RabbitMQOptions>() ?? throw new InvalidDataException(nameof(RabbitMQOptions));
        var clientProvidedName = ServiceInfo.Id;
        var version = ServiceInfo.Version;
        var groupName = $"cap.{ServiceInfo.ShortName}.{this.GetEnvShortName()}";
        Services.AddAdncInfraCap(subscribers, capOptions =>
                                 {
                                    SetCapBasicInfo(capOptions, version, groupName,failedThresholdCallback);
                                    SetCapRabbitMQInfo(capOptions, rabbitMQOptions, clientProvidedName);
                                    //需要引用DotNetCore.CAP.SqlServer
                                    option.UseSqlServer(config =>
                                    {
                                        config.ConnectionString = connectionString;
                                        config.Schema = "cap";
                                    });
                                 }, null, Lifetime);
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

- 工程文件中删除`Adnc.Infra.Repository.EfCore.Mysql.csproj`工程，并引用`Adnc.Infra.Repository.EfCore.SqlServer.csproj`。

## 部分切换

部分切换相对简单很多，只需要覆写`AddCapEventBus`，`AddEfCoreContext`即可。下面将以whse服务为例。

1、`Adnc.Whse.Application`工程引用`Adnc.Infra.Repository.EfCore.SqlServer.csproj`

```csharp
namespace Adnc.Whse.Application.Registrar;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    : AbstractApplicationDependencyRegistrar(services, serviceInfo, configuration, lifetime)
{        
    protected override void AddCapEventBus(IEnumerable<Type> subscribers, Action<FailedInfo>? failedThresholdCallback = null)
    {
        var connectionString = _configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var rabbitMQOptions = _configuration.GetRequiredSection(NodeConsts.RabbitMq).Get<RabbitMQOptions>() ?? throw new InvalidDataException(nameof(NodeConsts.RabbitMq));
        var clientProvidedName = _serviceInfo.Id;
        var version = _serviceInfo.Version;
        var groupName = $"cap.{_serviceInfo.ShortName}.{this.GetEnvShortName()}";
        _services.AddAdncInfraCap(subscribers, capOptions =>
        {
            SetCapBasicInfo(capOptions, version, groupName, failedThresholdCallback);
            SetCapRabbitMQInfo(capOptions, rabbitMQOptions, clientProvidedName);
            capOptions.UseSqlServer(sqlServerOptions =>
            {
                sqlServerOptions.ConnectionString = connectionString;
                sqlServerOptions.Schema = "cap";
            });
        }, null, _lifetime);
    }

    protected override void AddEfCoreContext()
    {
        AddOperater(_services);

        var connectionString = _configuration[NodeConsts.SqlServer_ConnectionString] ?? throw new InvalidDataException("SqlServer ConnectionString is null");
        var migrationsAssemblyName = _serviceInfo.MigrationsAssemblyName;
        _services.AddAdncInfraEfCoreSQLServer(RepositoryOrDomainLayerAssembly, optionsBuilder =>
        {
            optionsBuilder.UseLowerCaseNamingConvention();
            optionsBuilder.UseSqlServer(connectionString, optionsBuilder =>
            {
                optionsBuilder.MinBatchSize(4)
                                        .MigrationsAssembly(migrationsAssemblyName)
                                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }, _lifetime);
    }
}        
```



2、`Adnc.Whse.Migrations`工程引用`Adnc.Infra.Repository.EfCore.SqlServer.csproj`并删除`Adnc.Infra.Repository.EfCore.Mysql.csproj`工程。

3、注册SqlServer健康检测，`DependencyRegistrar.cs`

```csharp
namespace Adnc.Whse.WebApi.Registrar;

public sealed class DependencyRegistrar(IServiceCollection services, IServiceInfo serviceInfo, IConfiguration configuration) : AbstractWebApiDependencyRegistrar(services, serviceInfo, configuration)
{
    public override void AddAdncServices()
    {
        _services.AddHealthChecks(checksBuilder =>
        {
            checksBuilder
                    .AddSqlServer(connectionString) // sqlserver 
                    .AddRedis(redisSecton)
                    .AddRabbitMQ(rabbitSecton, clientProvidedName);
        });
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
—— 完 ——
