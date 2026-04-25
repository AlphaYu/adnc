# <div align="center"><img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-github.png" alt="ADNC-基于.NET平台的微服务开源框架" style="zoom:50%;" /></div>
<div align='center'>
<a href="./LICENSE">
<img alt="GitHub license" src="https://img.shields.io/github/license/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/stargazers">
<img alt="GitHub stars" src="https://img.shields.io/github/stars/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/network">
<img alt="GitHub forks" src="https://img.shields.io/github/forks/AlphaYu/Adnc"/>
</a>
<img alt="Visitors" src="https://komarev.com/ghpvc/?username=alphayu&color=red&label=Visitors"/>
</div>

###### <div align="center">代码改变世界，开源推动社区</div>

[中文](./README_ZH.md)  [English](./README.md)

## 简介

### ADNC 是什么？

`ADNC` 是一个基于 `.NET 8` 的开源分布式/微服务框架，也适用于单体架构项目。它围绕服务注册与发现、配置中心、链路追踪、负载均衡、熔断容错、分布式事务、分布式缓存、消息队列、`RPC` 调用（`HTTP` / `gRPC`）、认证授权、读写分离和日志记录等常见能力，提供了一套可直接落地的基础设施与工程实践。项目同时提供了配套文档和示例代码，便于理解框架设计并快速上手。

### 为什么选择 ADNC？

- 支持多种服务形态：既支持经典三层，也支持 `DDD` 和更紧凑的单项目服务结构。
- 基础设施开箱可用：围绕配置、注册发现、缓存、消息、认证、日志等常见需求提供现成集成方案。
- 适合学习与落地：仓库同时提供完整 Demo、配套文档和前端示例，便于理解整体架构和实践方式。
- 保持开放与可扩展：项目基于 `MIT` 许可证发布，可按需裁剪、扩展和集成到现有系统中。

无论是从零搭建新系统，还是梳理和演进现有项目，ADNC 都能作为一套可参考、可复用的工程基础。

## 快速开始

建议按下面顺序开始：

1. 先阅读 [快速开始文档](https://github.com/alphayu/adnc/wiki/02-quickstart-zh)
2. 使用 `src/Adnc.sln` 或 `src/Demo/Adnc.Demo.sln` 打开解决方案
3. 如需前端项目，请查看文末前端链接
4. 如需初始化数据，请查看文末数据库脚本链接

运行 Demo 前请先准备 `.NET 8 SDK`，以及快速开始文档中提到的基础依赖。更完整的接入与本地运行说明请直接查看快速开始文档。

## 目录 / 架构

### 目录结构

```
adnc 
├── .github
│   └── workflows CICD脚本目录(github-action)
├── doc 技术文档目录
├── src 源代码目录
│   ├── Infrastructures 基础架构层代码目录
│   ├── ServiceShared 服务通用层代码目录
│   ├── Gateways ocelot网关代码目录
│   └── Demo 示例代码目录
├── test 测试相关目录
├── .gitignore
├── README.md
└── LICENSE
```
### 重要文件
| 路径                                 | 描述                               |
| -------------------------------------| ---------------------------------- |
| `src/Adnc.sln`                      | 该解决方案包含`adnc`所有工程       |
| `src/Infrastructures/Adnc.Infra.sln`| 该解决方案仅包含基础架构层相关工程 |
| `src/ServiceShared/Adnc.Shared.sln` | 该解决方案仅包含服务通用层相关工程 |
| `src/Demo/Adnc.Demo.sln`            | 该解决方案仅包含`demo`相关工程     |
| `src/.editorconfig`            | 用于统一代码风格的跨编辑器配置文件，确保团队中无论谁用 VS、VS Code 还是 JetBrains Rider，写出的代码格式都是一致的     |
| `src/Directory.Build.props`             | 用于管理通用构建属性（如目标框架、语言版本、输出路径等）  |
| `src/Directory.Packages.props`          | 用于中央包管理 (CPM)，统一管理整个解决方案中 NuGet 包的版本号  |

### 总体架构图

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc_framework-e1682145003197.png" alt="adnc_framework"/>

#### Adnc.Infra.*

[NuGet Gallery | Packages matching adnc.infra](https://www.nuget.org/packages?q=adnc.infra)

![adnc-framework-2](https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-2.png)

#### Adnc.Shared.*

[NuGet Gallery | Packages matching adnc.shared](https://www.nuget.org/packages?q=adnc.shared)

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-3.png" alt="adnc-framework-3" style="zoom:80%;" />

### 技术栈

| 名称                                                         | 描述                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| <a target="_blank" href="https://github.com/ThreeMammals/Ocelot">Ocelot</a> | 基于 .NET 编写的开源网关                                     |
| <a target="_blank" href="https://github.com/hashicorp/consul">Consul</a> | 配置中心、注册中心组件                                       |
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a> | 声明式、类型安全的 RESTful 服务调用组件                      |
| <a target="_blank" href="https://github.com/grpc/grpc-dotnet">Grpc.Net.ClientFactory</a><br />Grpc.Tools | gRPC 通讯框架                                                |
| <a target="_blank" href="https://github.com/SkyAPM/SkyAPM-dotnet">SkyAPM.Agent.AspNetCore</a> | SkyWalking .NET 探针，链路追踪与性能监测组件                 |
| <a target="_blank" href="https://github.com/castleproject/Core">Castle DynamicProxy</a> | 动态代理，AOP开源实现组件                                    |
| <a target="_blank" href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo.EntityFrameworkCore.MySql</a> | EF Core ORM 组件                                             |
| <a target="_blank" href="https://github.com/StackExchange/Dapper">Dapper</a> | 轻量级 ORM 组件                                              |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a><br />NLog.MongoDB<br />NLog.Loki | 日志记录组件                                                 |
| <a target="_blank" href="https://github.com/AutoMapper/AutoMapper">AutoMapper</a> | 模型映射组件                                                 |
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | API 文档生成工具（Swagger）                                  |
| <a target="_blank" href="https://github.com/StackExchange/StackExchange.Redis">StackExchange.Redis</a> | Redis 客户端 SDK                                             |
| <a target="_blank" href="https://github.com/dotnetcore/CAP">CAP</a> | 事件总线与最终一致性（分布式事务）组件                       |
| <a target="_blank" href="https://github.com/rabbitmq/rabbitmq-dotnet-client">RabbitMQ</a> | 异步消息队列组件                                             |
| <a target="_blank" href="https://github.com/App-vNext/Polly">Polly</a> | .NET 弹性与瞬态故障处理库                                    |
| <a target="_blank" href="https://github.com/FluentValidation">FluentValidation</a> | .NET 验证框架                                                |
| <a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">MaxScale</a> | MariaDB 开发的一款成熟、高性能、免费开源的数据库中间件       |
| <a target="_blank" href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">AspNetCore.HealthChecks</a> | 健康检查组件，可与 Consul 健康检查配合使用                   |

## Demo 服务概览

Demo 提供了五个相互关联的微服务示例，分别展示了不同的服务拆分方式与工程组织形式。

| 服务  | 描述                                           | 架构风格                    |
| ----- | ---------------------------------------------- | --------------------------- |
| Admin | 系统管理（组织、用户、角色、权限、字典、配置） | 经典三层分离合约            |
| Maint | 运维管理（日志、审计）                         | 经典三层合并合约            |
| Cust  | 客户管理                                       | 单项目最小结构              |
| Ord   | 订单管理                                       | 领域驱动设计（DDD）带领域层 |
| Whse  | 仓库管理                                       | 领域驱动设计（DDD）带领域层 |

这些 Demo 展示了在保持整体框架一致性的前提下，如何按不同业务规模和复杂度组织代码。

##### :white_check_mark: Shared 

> Demo 公用工程，所有演示服务都会复用 `Shared` 目录中的通用组件。

```
Shared/
├── Remote.Event/ - 用于跨服务通信事件定义
├── Remote.Grpc/ - gRPC客户端定义
├── Remote.Http/ - HTTP客户端定义
├── protos/ - gRPC的协议文件定义
└── resources/ - 共享配置和资源
```

##### :white_check_mark: Adnc.Demo.Admin

> Admin 是系统管理服务，采用经典三层结构，并将应用服务契约定义拆分到 `Adnc.Demo.Admin.Application.Contracts` 层。这种组织方式层次清晰，适合边界明确、模块较多的后台管理场景。

```
Admin/
├── Api/ - 控制器和API端点
├── Application/ - 业务逻辑实现
├── Application.Contracts/ - DTO和服务接口
└── Repository/ - 数据访问层
```

##### :white_check_mark: Adnc.Demo.Maint

> Maint 是运维中心服务，采用更紧凑的三层结构，应用服务实现与契约定义都位于 `Adnc.Demo.Maint.Application` 层。这种结构减少了项目数量，同时保留了清晰的职责边界。

```
Maint/
├── Api/ - 控制器和端点
├── Application/ - 包含合约和实现
└── Repository/ - 数据访问层
```

##### :white_check_mark: Adnc.Demo.Cust

> Cust 是客户中心服务，采用单项目结构，控制器、应用服务、契约定义和仓储都位于同一个工程中。这种方式更适合职责单一、边界清晰的小型服务。

```
Cust/
└── Api/ - 包含控制器、应用逻辑和存储库
```

##### :white_check_mark: Adnc.Demo.Ord

> Ord 是订单中心服务，采用带独立领域层的 DDD 结构，用于突出业务规则与领域模型，并将其与应用层职责分离。

```
Ord/
├── Api/ - API端点
├── Application/ - 应用服务
├── Domain/ - 领域实体、聚合和领域服务
└── Migrations/ - 数据库迁移
```

##### :white_check_mark: Adnc.Demo.Whse

> Whse 是仓储中心服务，整体结构与 Ord 一致，同样采用带独立领域层的 DDD 组织方式。

```
Whse/
├── Api/ - API端点
├── Application/ - 应用服务
├── Domain/ - 领域实体、聚合和领域服务
└── Migrations/ - 数据库迁移
```

## 文档链接

| **序号** | 标题                                                         |
| -------- | ------------------------------------------------------------ |
| 1        | [ADNC 项目导览：一套可落地的 .NET 8 微服务/分布式工程实践](https://github.com/alphayu/adnc/wiki/01-adnc-intro-zh) |
| 2        | [ADNC 快速上手指南](https://github.com/alphayu/adnc/wiki/02-quickstart-zh) |
| 3        | [ADNC 快速 Docker 部署指南](https://github.com/alphayu/adnc/wiki/03-quickly-docker-deploy-zh) |
| 4        | [ADNC 配置节点详细说明](https://github.com/alphayu/adnc/wiki/04-appsettings-zh) |
| 5        | [ADNC 完整开发流程](https://github.com/alphayu/adnc/wiki/05-feature-dev-guide-zh) |
| 6        | [ADNC Repository 层开发指引](https://github.com/alphayu/adnc/wiki/06-repository-dev-guide-zh) |
| 7        | [ADNC Service层开发指引](https://github.com/alphayu/adnc/wiki/07-service-dev-guide) |
| 8        | [ADNC API 层开发指引](https://github.com/alphayu/adnc/wiki/08-api-dev-guide-zh) |
| 9        | [ADNC 如何认证与授权](https://github.com/alphayu/adnc/wiki/09-claims-based-authentication-zh) |
| 10       | [ADNC 如何使用仓储 - 基础功能](https://github.com/alphayu/adnc/wiki/10-efcore-pemelo-curd-zh) |
| 11       | [ADNC 如何使用仓储 - CodeFirst](https://github.com/alphayu/adnc/wiki/11-efcore-pemelo-codefirst-zh) |
| 12       | [ADNC 如何使用仓储 - 切换数据库类型](https://github.com/alphayu/adnc/wiki/12-efcore-pemelo-sqlserver-zh) |
| 13       | [ADNC 如何使用仓储 - 事务](https://github.com/alphayu/adnc/wiki/13-efcore-pemolo-unitofwork-zh) |
| 14       | [ADNC  如何使用仓储 - 执行原生SQL](https://github.com/alphayu/adnc/wiki/14-efcore-pemelo-sql-zh) |
| 15       | [ADNC 如何使用仓储 - 读写分离](https://github.com/alphayu/adnc/wiki/15-maxsale-readwritesplit-zh) |
| 16       | [ADNC Id生成器(雪花算法)介绍](https://github.com/alphayu/adnc/wiki/16-snowflake-max_value-wokerid-zh) |
| 17       | [ADNC 如何使用Cache/Redis/分布式锁/布隆过滤器](https://github.com/alphayu/adnc/wiki/17-cache-redis-distributedlock-bloomfilter-zh) |
| 18       | [ADNC 服务之间如何通过 HTTP 调用（Refit）](https://github.com/alphayu/adnc/wiki/18-service-http-call-zh) |
| 19       | [ADNC 服务之间如何通过 gRPC 调用](https://github.com/alphayu/adnc/wiki/19-service-grpc-call-zh) |
| 20       | [ADNC 服务之间如何通过事件（CAP）通信](https://github.com/alphayu/adnc/wiki/20-service-event-call-zh) |
| 21       | [ADNC 如何开启 SkyAPM（SkyWalking）链路追踪](https://github.com/alphayu/adnc/wiki/21-skyapm-tracing-zh) |
| 22       | [ADNC 如何使用配置中心（Consul）](https://github.com/alphayu/adnc/wiki/22-config-center-zh) |
| 23       | [ADNC 如何使用注册中心](https://github.com/alphayu/adnc/wiki/23-registry-center-zh) |

## 截图 / JMeter / 官网

### JMeter测试

> 6个测试用例覆盖了网关、服务发现、配置中心、服务间同步调用、数据库 CRUD、本地事务、分布式事务、缓存、布隆过滤器、SkyAPM 链路、NLog 日志记录、操作日志记录。

- ECS服务器配置：4核8G，带宽8M。服务器上装了很多东西，剩余大约50%的CPU资源，50%的内存资源。
- 因为服务器带宽限制，吞吐率约1000/s左右。
- 模拟并发线程1200/s
- 读写比率7:3

### 前端

基于 Vue 3、Vite、TypeScript 和 Element Plus 的开箱即用后台管理前端模板。

#### 项目地址

- [adnc-vue3: ADNC's Vue3 front-end](https://github.com/alphayu/adnc-vue-elementplus)

#### 界面截图

![.NET微服务开源框架-异常日志界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-nlog.png)
![.NET微服务开源框架-角色管理界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-role.png)

### 相关链接

#### 项目官网

- [https://aspdotnetcore.net](https://aspdotnetcore.net)

#### 演示网址

- [https://online.aspdotnetcore.net](https://online.aspdotnetcore.net)

#### 代码生成器

- [https://code.aspdotnetcore.net](https://code.aspdotnetcore.net)

#### 数据库脚本

- [adnc/doc/dbsql at develop · AlphaYu/adnc](https://github.com/AlphaYu/adnc/tree/develop/doc/dbsql)

### 问题交流

- QQ群号：780634162

- 都看到这里了，那就点个`star`吧！

## License

本项目基于 **MIT License** 开源，详见 [LICENSE](./LICENSE)。
