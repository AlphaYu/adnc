# <div align="center"><img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-github.png" alt="ADNC-基于.NET平台的微服务开源框架" style="zoom:50%;" /></div>
<div align='center'>
<a href="https://github.com/AlphaYu/Adnc/blob/master/LICENSE">
<img alt="GitHub license" src="https://img.shields.io/github/license/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/stargazers">
<img alt="GitHub stars" src="https://img.shields.io/github/stars/AlphaYu/Adnc"/>
</a>
<a href="https://github.com/AlphaYu/Adnc/network">
<img alt="GitHub forks" src="https://img.shields.io/github/forks/AlphaYu/Adnc"/>
</a>
<a href="">
<img alt="Visitors" src="https://komarev.com/ghpvc/?username=alphayu&color=red&label=Visitors"/>
</a>
</div>

###### <div align="center">代码改变世界，开源推动社区</div>

## 概述
### ADNC是什么？

`ADNC` 是一个基于 `.NET` 平台的分布式/微服务开源框架，采用现代化的架构设计和最佳实践，同时也适用于单体架构系统的开发。它提供了一系列的工具和库，帮助开发人员快速构建和部署微服务应用程序，包括服务注册/发现、配置中心、链路跟踪、负载均衡、熔断、容错、分布式事务、分布式缓存、消息队列、`RPC`调用(`http`/`grpc`)、认证授权、读写分离、日志记录等，同时也提供了完善的文档和示例代码，方便开发人员使用和学习。如果您正在考虑使用分布式/微服务或单体架构开发应用程序，`ADNC`  框架是一个值得尝试的开源框架。

> 微服务是一种分布式架构模式，通过将应用程序拆分成一组小型、松耦合的服务，可以提高应用程序的可伸缩性、可靠性和灵活性。

### ADNC有什么优点?

- 灵活性：框架采用现代化的架构设计，支持经典三层和`DDD`架构开发模式。
- 易用性：框架提供了完善的文档和示例代码，同时也集成了一系列主流的微服务技术栈，使用起来比较容易上手。
- 高可靠性：框架采用容器化部署、负载均衡、服务发现等技术，可以提高应用程序的可靠性和可伸缩性。
- 开放性：框架是一个开源项目，采用 `MIT` 许可证发布，用户可以自由地使用、修改和分享该框架的源代码。
- 生态圈：框架的社区生态圈正在逐渐壮大，有越来越多的开发人员在使用和贡献该框架，用户可以从社区中获取到更多的资源和支持。

## 架构设计

### 目录结构

```
adnc 
├── .github
│   └── workflows CICD脚本目录(github-action)
├── doc 技术文档目录
├── src 源代码目录
│   ├── ServerApi 后端代码目录
│   │   ├── Infrastructures 基础架构层代码目录
│   │   ├── ServiceShared 服务通用层代码目录
│   │   ├── Gateways ocelot网关代码目录
│   │   └── Demo 示例代码目录
│   └── ClientApp 前端代码目录
├── test 测试相关目录
├── .gitignore
├── README.MD
└── LICENSE
```
### 重要文件
| 路径                                           | 描述                               |
| ---------------------------------------------- | ---------------------------------- |
| `src/ServerApi/Adnc.sln`                       | 该解决方案包含`adnc`所有工程       |
| `src/ServerApi/Infrastructures/Adnc.Infra.sln` | 该解决方案仅包含基础架构层相关工程 |
| `src/ServerApi/ServiceShared/Adnc.Shared.sln`  | 该解决方案仅包含服务通用层相关工程 |
| `src/ServerApi/Demo/Adnc.Demo.sln`             | 该解决方案仅包含`demo`相关工程     |
| `scr/ServerApi/common.props`                   | 工程文件`*.csproj`公用配置         |
| `scr/ServerApi/version_infra.props`            | 基础架构层版本号                   |
| `scr/ServerApi/version_shared.props`           | 服务通用层版本号                   |
| `scr/ServerApi/nuget.props`                    | `Nuget`发布信息配置                |
### 总体架构图

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc_framework-e1682145003197.png" alt="adnc_framework"/>

#### Adnc.Infra.*

[NuGet Gallery | Packages matching adnc.infra](https://www.nuget.org/packages?q=adnc.infra)

![adnc-framework-2](https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-2.png)

#### Adnc.Shared.*

[NuGet Gallery | Packages matching adnc.shared](https://www.nuget.org/packages?q=adnc.shared)

<img src="https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-framework-3.png" alt="adnc-framework-3" style="zoom:80%;" />

### 解决方案截图

![adnc-solution](https://aspdotnetcore.net/wp-content/uploads/2023/04/adnc-solution.png)

### 技术栈

| 名称                                                         | 描述                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| <a target="_blank" href="https://github.com/ThreeMammals/Ocelot">Ocelot</a> | 基于 `.NET6 编写的开源网关                                   |
| <a target="_blank" href="https://github.com/hashicorp/consul">Consul</a> | 配置中心、注册中心组件                                       |
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a> | 一个声明式自动类型安全的RESTful服务调用组件，用于同步调用其他微服务 |
| <a target="_blank" href="https://github.com/grpc/grpc-dotnet">Grpc.Net.ClientFactory</a><br />Grpc.Tools | Grpc通讯框架                                                 |
| <a target="_blank" href="https://github.com/SkyAPM/SkyAPM-dotnet">SkyAPM.Agent.AspNetCore</a> | Skywalking `.NET6探针，性能链路监测组件                      |
| <a target="_blank" href="https://github.com/castleproject/Core">Castle DynamicProxy</a> | 动态代理，AOP开源实现组件                                    |
| <a target="_blank" href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo.EntityFrameworkCore.MySql</a> | EFCore ORM组件                                               |
| <a target="_blank" href="https://github.com/StackExchange/Dapper">Dapper</a> | 轻量级ORM组件                                                |
| <a target="_blank" href="https://entityframework-plus.net">Z.EntityFramework.Plus.EFCore</a> | 第三方高性能的EfCore组件                                     |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a><br />Nlog.Mongdb<br />Nlog.Loki | 日志记录组件                                                 |
| <a target="_blank" href="https://github.com/AutoMapper/AutoMapper">AutoMapper</a> | 模型映射组件                                                 |
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | APIs文档生成工具(swagger)                                    |
| <a target="_blank" href="https://github.com/StackExchange/StackExchange.Redis">StackExchange.Redis</a> | 开源的Redis客户端SDK                                         |
| <a target="_blank" href="https://github.com/dotnetcore/CAP">CAP</a> | 实现事件总线及最终一致性（分布式事务）的一个开源的组件       |
| <a target="_blank" href="https://github.com/rabbitmq/rabbitmq-dotnet-client">RabbitMq</a> | 异步消息队列组件                                             |
| <a target="_blank" href="https://github.com/App-vNext/Polly">Polly</a> | 一个 .NET 弹性和瞬态故障处理库，允许开发人员以 Fluent 和线程安全的方式来实现重试、断路、超时、隔离和回退策略 |
| <a target="_blank" href="https://github.com/FluentValidation">FluentValidation</a> | 一个 .NET 验证框架，支持链式操作，易于理解，功能完善，组件内提供十几种常用验证器，可扩展性好，支持自定义验证器，支持本地化多语言 |
| <a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">Maxscale</a> | Mariadb开发的一款成熟、高性能、免费开源的数据库中间件        |
| <a target="_blank" href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">AspNetCore.HealthChecks</a> | 健康监测组件,搭配consul的健康监测                            |

## 文档

#### 如何快速跑起来
- 详细介绍如何使用docker安装redis、mysql、rabbitmq、mongodb，以及如何在本地配置ClientApp、ServerApi。<br/>
[请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/quickstart/)

#### 如何手动部署到容器
- 详细介绍如何使用docker安装配置consul集群、Skywalking系列组件、相关项目的dockerfile文件编写与配置以及如何将多个服务部署到服务器。<br/>
  [请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/deploy-docker/)

#### 如何使用jenkins+shell脚本自动化部署
- 文档尚未完成

#### 如何部署到K8S
- 文档尚未完成

#### 如何实现读写分离
- 详细介绍为什么要通过中间件实现读写分离以及EFCore基于中间件如何写代码。<br/>
[请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/maxsale-readwritesplit/)

#### 如何使用Cache Redis 分布式锁 布隆过滤器
- 详细介绍如何使用Cache、Redis、分布式锁以及布隆过滤器。如何配置Cache防止雪崩、击穿、穿透以及缓存同步。<br/>
[请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/cache-redis-distributedlock-bloomfilter/)

#### 如何动态分配雪花算法的WorkerId
- 详细介绍Yitter雪花算法的特点、配置以及如何动态获取WorkerId。<br/>
[请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/snowflake-max_value-wokerid/)

#### 如何认证与授权
- 详细介绍为什么要采用JwtBearer+Basic混合认证模式以及它们的实现逻辑，如何灵活配置与应用。  
[请点击链接，查看详细介绍](https://aspdotnetcore.net/docs/claims-based-authentication/)
#### 如何使用EFCore仓储
- 详细介绍EFCore仓储基础功能、工作单元、CodeFirst，执行原生SQL等提供了丰富的演示代码以及演示代码对应的Sql语句。
1. [如何使用仓储(一)-基础功能](https://aspdotnetcore.net/docs/efcore-pemelo-grud/)<br/>
1. [如何使用仓储(二)-分布式事务/本地事务](https://aspdotnetcore.net/docs/efcore-pemolo-unitofwork/)<br/>
1. [如何使用仓储(三)-CodeFirst](https://aspdotnetcore.net/docs/efcore-pemelo-codefirst/)<br/>
1. [如何使用仓储(四)-撸SQL](https://aspdotnetcore.net/docs/efcore-pemelo-sql/)<br/>
1. [ 如何使用仓储(五)-切换数据库类型](https://aspdotnetcore.net/docs/efcore-pemelo-sqlserver/)<br/>

#### 如何从零开发业务
- 文档尚未完成

#### 如何调用微服务
- 文档尚未完成

#### 如何配置网关
- 文档尚未完成

#### 如何使用注册/配置中心
- 文档尚未完成

#### 如何配置链路追踪
- 文档尚未完成

#### 如何配置健康检测
- 文档尚未完成

## Demo介绍

##### :white_check_mark: Shared 

> Demo公用工程

- `protos` grpc定义文件
- `resources` 公用的静态文件与配置文件
- `Adnc.Demo.Shared.Const` 常量文件
- `Adnc.Demo.Shared.Rpc.Event` 事件文件
- `Adnc.Demo.Shared.Rpc.Grpc` gprc客户端
- `Adnc.Demo.Shared.Rpc.Http` http客户端

##### :white_check_mark: Adnc.Demo.Usr

> 经典三层开发模式，剥离了应用服务协议定义文件到`Adnc.Demo.Usr.Application.Contracts`层

用户中心服务是系统支撑服务，实现了用户管理、角色管理、权限管理、菜单管理、组织架构管理。

##### :white_check_mark: Adnc.Demo.Maint

> 经典三层开发模式，应用服务实现与协议定义都在`Adnc.Demo.Maint.Application`层

运维中心服务是系统支撑服务，实现了登录日志、审计日志、异常日志、字典管理、配置参数管理。

##### :white_check_mark: Adnc.Demo.Cus

> 经典三层开发模式，控制器、应用服务实现与协议定义、仓储都在同一个工程，这种结构适合细粒度服务拆分模式。

客户中心微服务。

##### :white_check_mark: Adnc.Demo.Ord

> DDD开发模式

订单中心微服务。

##### :white_check_mark: Adnc.Demo.Whse

> DDD开发模式

仓储中心微服务。

## Jmeter测试

> 6个测试用例覆盖了网关、服务发现、配置中心、服务间同步调用、数据库CURD、本地事务、分布式事务、缓存、布隆过滤器、SkyApm链路、Nlog日志记录、操作日志记录。

- ECS服务器配置：4核8G，带宽8M。服务器上装了很多东西，剩余大约50%的CPU资源，50%的内存资源。
- 因为服务器带宽限制，吞吐率约1000/s左右。
- 模拟并发线程1200/s
- 读写比率7:3

## 前端

### 项目地址

- [adnc-vue2: ADNC's Vue2 front-end](https://github.com/alphayu/adnc-vue2)
- [adnc-vue3: ADNC's Vue3 front-end](https://github.com/alphayu/adnc-vue3)

### 界面截图

![.NET微服务开源框架-异常日志界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-nlog.png)
![.NET微服务开源框架-角色管理界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-role.png)

## 其他

### 项目官网
- [https://aspdotnetcore.net](https://aspdotnetcore.net)

### 演示地址
- [http://adnc.aspdotnetcore.net](http://adnc.aspdotnetcore.net)
### 问题交流
- QQ群号：780634162

- 都看到这里了，那就点个`star`吧！

## License

**MIT**   
**Free Software, Hell Yeah!**
