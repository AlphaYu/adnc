## 前言

在开发环境中，`adnc` 所有服务的公共配置位于 `adnc\src\Demo\Shared\resources\appsettings.shared.Development.json` 文件中，包含诸如 `Redis`、`RabbitMQ` 等配置。各服务特有的配置则存储在 `api` 工程的 `appsettings.Development` 文件中，例如数据库、服务端口等。

在生产环境中，建议将所有配置存储到 Consul 配置中心。

## 配置节点说明

### 1. 服务公共配置

>adnc\src\Demo\Shared\resources\appsettings.shared.Development.json

#### 1.1 RegisterType

- 服务注册类型
  - `Direct`：不注册，服务间调用通过 URI 地址，参考 `RpcInfo` 节点。
  - `Consul`：注册到 Consul，服务间调用通过服务名，参考 `RpcInfo` 节点。
  - `CoreDns`：注册到 K8S，服务间调用通过 K8S 内部域名，参考 `RpcInfo` 节点。

```json
"RegisterType": "Direct"
```

#### 1.2 Basic

- `Basic` 认证信息，服务间调用采用 `Basic` 认证
  - `UserName`：用户名
  - `Password`：密码

```json
"Basic": {
    "UserName": "adnc",
    "Password": "yvMRER0wzSStw2Va0z59PNQd0lqeMYIP"
}
```

#### 1.3 RpcInfo

- 服务调用信息
  - `Polly:Enable`：是否开启 `Polly` 策略
    - `true`：开启
    - `false`：关闭
  - `Address`：服务地址信息
    - `Service`：服务名
    - `Direct`：`RegisterType = Direct` 对应的地址
    - `Consul`：`RegisterType = Consul` 对应的地址
    - `CoreDns`：`RegisterType = CoreDns` 对应的地址

```json
"RpcInfo": {
    "Polly": {
        "Enable": false
    },
    "Address": [
        {
            "Service": "adnc-demo-admin-api",
            "Direct": "http://localhost:50010",
            "Consul": "http://adnc-demo-admin-api",
            "CoreDns": "http://adnc-demo-admin-api.default.svc.cluster.local"
        },
        {
            "Service": "adnc-demo-maint-api",
            "Direct": "http://localhost:50020",
            "Consul": "http://adnc-demo-maint-api",
            "CoreDns": "http://adnc-demo-maint-api.default.svc.cluster.local"
        },
        {
            "Service": "adnc-demo-cust-api",
            "Direct": "http://localhost:50030",
            "Consul": "http://adnc-demo-cust-api",
            "CoreDns": "http://adnc-demo-cust-api.default.svc.cluster.local"
        }
    ]
}
```

#### 1.4 Redis

- `Redis` 信息
  - `Provider`：客户端驱动，目前仅支持 `StackExchange`
  - `EnableLogging`：是否开启日志
  - `SerializerName`：序列化方式，支持 `json`、`binary`、`proto`
  - `EnableBloomFilter`：是否允许布隆过滤器
  - `Dbconfig:ConnectionString`：连接字符串

```json
"Redis": {
    "Provider": "StackExchange",
    "EnableLogging": true,
    "SerializerName": "json",
    "EnableBloomFilter": false,
    "Dbconfig": {
        "ConnectionString": "62.234.187.128:13379,password=football,defaultDatabase=0,ssl=false,sslHost=null,connectTimeout=4000,allowAdmin=true"
    }
}
```

#### 1.5 Caching

- 缓存信息，`cache` 依赖 `redis` 实现
  - `MaxRdSecond`：当 `cache` 保存时，过期时间会加上最大不超过 `MaxRdSecond` 的一个随机数，防止缓存雪崩。
  - `LockMs`：获取分布式锁的锁定时间。
  - `SleepMs`：未能获取分布式锁的休眠时间。
  - `EnableLogging`：是否开启日志。
  - `PollyTimeoutSeconds`：`Polly` 超时时间，`cache` 与数据库同步补偿机制会用到这个参数。
  - `PenetrationSetting`：缓存穿透配置。
    - `Disable`：是否禁用
    - `BloomFilterSetting`：布隆过滤器配置
      - `Name`：名字
      - `Capacity`：容量
      - `ErrorRate`：误报概率

```json
"Caching": {
    "MaxRdSecond": 30,
    "LockMs": 6000,
    "SleepMs": 300,
    "EnableLogging": true,
    "PollyTimeoutSeconds": 11,
    "PenetrationSetting": {
        "Disable": true,
        "BloomFilterSetting": {
            "Name": "adnc:$SHORTNAME:bloomfilter:cachekeys",
            "Capacity": 10000000,
            "ErrorRate": 0.001
        }
    }
}
```

#### 1.6 RabbitMQ

- `RabbitMQ` 信息
  - `HostName`：主机地址
  - `Port`：主机端口
  - `VirtualHost`：虚拟主机名
  - `UserName`：用户名
  - `Password`：密码

```json
"RabbitMq": {
    "HostName": "62.234.187.128",
    "Port": "5672",
    "VirtualHost": "/",
    "UserName": "admin",
    "Password": "football"
}
```

#### 1.7 SysLogDb

- 登录/审计日志数据库信息
  - `DbType`：数据库类型，支持 `mysql`、`sqlserver`、`oracle`
  - `ConnectionString`：连接字符串

```json
"SysLogDb": {
    "DbType": "mysql",
    "ConnectionString": "Server=62.234.187.128;Port=13308;database=adnc_syslog;uid=root;pwd=alpha.netcore;connection timeout=30;"
}
```

#### 1.8 Consul

- `Consul` 公共信息
  - `ServiceName`：服务名占位符
  - `ServerTags`：服务 tags
  - `HealthCheckUrl`：健康检查地址
  - `HealthCheckIntervalInSecond`：健康检查间隔
  - `DeregisterCriticalServiceAfter`：健康检查失败后，服务注销时间
  - `Timeout`：超时时间

```json
"Consul": {
    "ServiceName": "$SERVICENAME",
    "ServerTags": [ "urlprefix-/$SHORTNAME" ],
    "HealthCheckUrl": "$RELATIVEROOTPATH/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb",
    "HealthCheckIntervalInSecond": 6,
    "DeregisterCriticalServiceAfter": 20,
    "Timeout": 6
}
```

#### 1.9 Logging

- `Logger` 信息
  - `Logging`
    - `LogContainer`: 日志记录方式，支持 `console`、`file`、`loki`，分别对应 `adnc\src\Demo\Shared\resources\NLog` 目录下的三个配置文件。
    - `LogLevel`: 日志级别
  - `Loki`：当 `Logging:LogContainer = loki` 时，会从该节点读取 `Loki` 连接信息。

```json
"Logging": {
    "IncludeScopes": true,
    "LogContainer": "console",
    "LogLevel": {
        "Default": "Information",
        "Adnc": "Debug",
        "Microsoft": "Information"
    }
},
"Loki": {
    "Endpoint": "http://10.2.8.5:3100",
    "UserName": "",
    "Password": ""
}
```

#### 1.10 CorsHosts

- 浏览器跨域域名信息，`*` 表示允许所有域名。

```json
"CorsHosts": "*"
```

#### 1.11 JWT

- `Jwt token` 创建与认证规则

```json
"JWT": {
    "ValidateIssuer": true,
    "ValidIssuer": "adnc",
    "ValidateIssuerSigningKey": true,
    "SymmetricSecurityKey": "alphadotnetcoresecurity24b010055e0f2e9564fb",
    "ValidateAudience": true,
    "ValidAudience": "manager",
    "ValidateLifetime": true,
    "RequireExpirationTime": true,
    "ClockSkew": 1,
    "RefreshTokenAudience": "manager",
    "Expire": 6000,
    "RefreshTokenExpire": 10080
}
```

#### 1.12 SkyWalking

- 链路跟踪客户端信息
  - `ServiceName` 服务名

```json
"SkyWalking": {
    "ServiceName": "$SERVICENAME",
    "Namespace": "adnc",
    "HeaderVersions": [
        "sw8"
    ],
    "Sampling": {
        "SamplePer3Secs": -1,
        "Percentage": -1.0,
        "IgnorePaths": [ "/*/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb", "http://**/appsettings", "/**/swagger.json", "http://**/loki/api/v1/push" ]
    },
    "Logging": {
        "Level": "Error",
        "FilePath": "txtlogs\\skyapm-{Date}.log"
    },
    "Transport": {
        "Interval": 3000,
        "ProtocolVersion": "v8",
        "QueueSize": 30000,
        "BatchSize": 3000,
        "gRPC": {
            "Servers": "62.234.187.128:11800",
            "Timeout": 10000,
            "ConnectTimeout": 10000,
            "ReportTimeout": 600000,
            "Authentication": ""
        }
    }
}
```

### 2. 单个服务配置

> 单个服务配置以 Admin.Api 服务的开发环境和生产环境为例
> adnc\src\Demo\Admin.Api\appsettings.Development.json
> adnc\src\Demo\Admin.Api\appsettings.Production.json

#### 2.1 `appsettings.Development.json`

- `ConfigurationType` 配置文件类型
  - `ConfigurationType = File`：服务启动时加载 `appsettings.shared.Development.json`。
- `Mysql` 业务数据库连接信息
- `Kestrel` 服务 URL 与端口信息

```json
{
    "ConfigurationType": "File",
    "Mysql": {
        "ConnectionString": "Server=127.0.0.1;Port=13308;database=adnc_admin;uid=root;pwd=alpha.netcore;connection timeout=30;"
    },
    "Kestrel": {
        "Endpoints": {
            "Default": {
                "Url": "http://0.0.0.0:50010"
            },
            "Grpc": {
                "Url": "http://0.0.0.0:50011",
                "Protocols": "Http2"
            }
        }
    }
}
```

#### 2.2 `appsettings.Production.json`

- `ConfigurationType` 配置文件类型
  - `ConfigurationType = Consul`：服务启动时从 Consul 获取配置。
- `Consul`
  - `ConsulUrl`：Consul 地址
  - `ConsulKeyPath`：配置信息 Key 的路径

```json
{
  "ConfigurationType": "Consul",
  "Consul": {
    "ConsulUrl": "http://10.2.8.5:8500",
    "ConsulKeyPath": "adnc/production/shared/appsettings,adnc/production/$SHORTNAME/appsettings"
  }
}
```

#### 2.3 获取配置相关代码

```csharp
public static WebApplicationBuilder AddConfiguration(this WebApplicationBuilder builder, IServiceInfo serviceInfo)
{
    var configurationType = builder.Configuration.GetValue<string>(NodeConsts.ConfigurationType) ?? ConfigurationTypeConsts.File;
    switch (configurationType)
    {
        case ConfigurationTypeConsts.File:
            builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}/appsettings.shared.{builder.Environment.EnvironmentName}.json", true, true);
            break;
        case ConfigurationTypeConsts.Consul:
            var consulOption = builder.Configuration.GetSection(NodeConsts.Consul).Get<ConsulOptions>();
            if (consulOption is null || consulOption.ConsulKeyPath.IsNullOrWhiteSpace())
            {
                throw new NotImplementedException(NodeConsts.Consul);
            }
            else
            {
                consulOption.ConsulKeyPath = consulOption.ConsulKeyPath.Replace("$SHORTNAME", serviceInfo.ShortName);
                builder.Configuration.AddConsulConfiguration(consulOption, true);
            }
            break;
        case ConfigurationTypeConsts.Nacos:
            throw new NotImplementedException(nameof(ConfigurationTypeConsts.Nacos));
        case ConfigurationTypeConsts.Etcd:
            throw new NotImplementedException(nameof(ConfigurationTypeConsts.Etcd));
        default:
            throw new NotImplementedException(nameof(configurationType));
    }
    
    // ....

    return builder;
}
```

------

WELL DONE，记得 star && fork。
 全文完，[ADNC](https://aspdotnetcore.net/) 一个可以落地的 .NET 微服务/分布式开发框架。