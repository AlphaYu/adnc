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
</div>

# <div align="center">![Adnc是一个微服务开发框架 代码改变世界 开源推动社区](https://aspdotnetcore.net/wp-content/uploads/2022/05/adnc-repository-open-graph-e1652098599436.png)</div>
## <div align="center">*代码改变世界，开源推动社区*</div>
&ensp;&ensp;&ensp;&ensp;<a target="_blank" title="一个轻量级的.Net 5.0微服务开发框架" href="https://aspdotnetcore.net">Adnc</a>是一个轻量级的完全可以落地的微服务/分布式开发框架，同时也适用于单体架构系统的开发。支持经典三层与DDD架构开发模式、集成了一系列主流稳定的微服务配套技术栈。一个前后端分离的框架，前端基于<a target="_blank" href="https://github.com/vuejs/vue">Vue</a>、后端基于<a target="_blank" href="https://github.com/dotnet/core">.Net6</a>构建。WebApi遵循RESTful设计规范、基于JWT认证授权、基于<a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">Maxscale</a>实现了读写分离、部署灵活、代码简洁、开箱即用、容器化微服务的最佳实践。

- 用户中心：系统支撑服务，实现了用户管理、角色管理、权限管理、菜单管理、组织架构管理
- 运维中心：系统支撑服务，实现了登录日志、审计日志、异常日志、字典管理、配置参数管理
- 客户中心：经典三层开发模式demo
- 订单中心：DDD开发模式demo
- 仓储中心：DDD开发模式demo

## 问题交流

- QQ群：780634162
- 项目官网：<a target="_blank" href="https://aspdotnetcore.net">https://aspdotnetcore.net</a>
- 博&ensp;&ensp;&ensp;&ensp;客：<a target="_blank" href="https://www.cnblogs.com/alphayu">https://www.cnblogs.com/alphayu</a></a>

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

## 总体结构设计
- 经典三层
![.NET微服务开源框架-总体设计](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-traditional.png)
- DDD三层
![.NET微服务开源框架-总体设计](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-ddd.png)
- 总体结构
![.NET微服务开源框架-总体设计](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-rpc-eventbus.png)

## 代码片段

```csharp
internal static class Program
{
    internal static async Task Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        logger.Debug($"init {nameof(Program.Main)}");
        try
        {
            var webApiAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var serviceInfo = Shared.WebApi.ServiceInfo.CreateInstance(webApiAssembly);
            
            var app = WebApplication
                .CreateBuilder(args)
                .ConfigureAdncDefault(args, serviceInfo)
                .Build();

            app.UseAdncDefault(endpointRoute: endpoint =>
            {
                endpoint.MapGrpcService<Grpc.MaintGrpcServer>();
            });

            await app
                .ChangeThreadPoolSettings()
                .UseRegistrationCenter()
                .RunAsync();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }
}
```

## Jmeter测试

- ECS服务器配置：4核8G，带宽8M。服务器上装了很多东西，剩余大约50%的CPU资源，50%的内存资源。
- 因为服务器带宽限制，吞吐率约1000/s左右。
- 模拟并发线程1200/s
- 读写比率7:3

> 6个测试用例覆盖了网关、服务发现、配置中心、服务间同步调用、数据库CURD、本地事务、分布式事务、缓存、布隆过滤器、SkyApm链路、Nlog日志记录、操作日志记录。

## 演示
- <a href="http://adnc.aspdotnetcore.net" target="_blank">http://adnc.aspdotnetcore.net</a>

## GitHub
- <a href="https://github.com/alphayu/adnc" target="_blank">https://github.com/alphayu/adnc</a>
- 开源不易，如果您喜欢这个项目, 请给个星星⭐️。

## 路线图
  - [计划完善与新增的模块](https://docs.qq.com/doc/DY2hrYkFYVEl5YW9Y)

## 目录结构
  - src
    - clientApp 前端项目(`Vue`)
    - serverApi 后端项目(`.NET6.0`)
  - doc 项目相关文档(sql脚本、docker脚本、docker-compose.yaml文件)
  - tools 工具软件  
  - test 测试工程
#### ClientApp
  - clientApp基于<a target="_blank" href="https://github.com/PanJiaChen/vue-element-admin">Vue-Element-Admin</a>以及<a target="_blank" href="https://github.com/enilu/web-flash">Web-Flash</a>搭建，感谢两位作者。
  - 前端主要技术栈 Vue + Vue-Router + Vuex + Axios
  - 构建步骤
    ```bash 
    # Install dependencies 
    npm install --registry=https://registry.npm.taobao.org
    # Serve with hot reload at localhost:5001
    npm run dev
    # Build for production with minification
    npm run build:prod
    ```
  - 界面
![.NET微服务开源框架-异常日志界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-nlog.png)
![.NET微服务开源框架-角色管理界面](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc-dashboard-role.png)

#### ServerApi
  - ServerApi基于`.NET 5.0`搭建。
  - 后端主要技术栈

| 名称 | 描述 |
| ---- | -----|
| <a target="_blank" href="https://github.com/ThreeMammals/Ocelot">Ocelot</a> | 基于 `.NET6 编写的开源网关 |
| <a target="_blank" href="https://github.com/hashicorp/consul">Consul</a> | 配置中心、注册中心组件|
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a>  | 一个声明式自动类型安全的RESTful服务调用组件，用于同步调用其他微服务|
| [Grpc.Net.ClientFactory]([grpc/grpc-dotnet: gRPC for .NET (github.com)](https://github.com/grpc/grpc-dotnet))<br />Grpc.Tools | Grpc通讯框架 |
| <a target="_blank" href="https://github.com/SkyAPM/SkyAPM-dotnet">SkyAPM.Agent.AspNetCore</a> | Skywalking `.NET6探针，性能链路监测组件 |
| <a target="_blank" href="https://github.com/castleproject/Core">Castle DynamicProxy</a> | 动态代理，AOP开源实现组件 |
| <a target="_blank" href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo.EntityFrameworkCore.MySql</a> | EFCore ORM组件 |
| <a target="_blank" href="https://github.com/StackExchange/Dapper">Dapper</a> | 轻量级ORM组件 |
| <a target="_blank" href="https://entityframework-plus.net">Z.EntityFramework.Plus.EFCore</a> | 第三方高性能的EfCore组件 |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a><br />Nlog.Mongdb<br />Nlog.Loki | 日志记录组件 |
| <a target="_blank" href="https://github.com/AutoMapper/AutoMapper">AutoMapper</a> | 模型映射组件 |
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | APIs文档生成工具(swagger) |
| <a target="_blank" href="https://github.com/StackExchange/StackExchange.Redis">StackExchange.Redis</a> | 开源的Redis客户端SDK |
| <a target="_blank" href="https://github.com/dotnetcore/CAP">CAP</a>  | 实现事件总线及最终一致性（分布式事务）的一个开源的组件 |
| <a target="_blank" href="https://github.com/rabbitmq/rabbitmq-dotnet-client">RabbitMq</a>  | 异步消息队列组件 |
| <a target="_blank" href="https://github.com/App-vNext/Polly">Polly</a>  | 一个 .NET 弹性和瞬态故障处理库，允许开发人员以 Fluent 和线程安全的方式来实现重试、断路、超时、隔离和回退策略 |
| <a target="_blank" href="https://github.com/FluentValidation">FluentValidation</a>  | 一个 .NET 验证框架，支持链式操作，易于理解，功能完善，组件内提供十几种常用验证器，可扩展性好，支持自定义验证器，支持本地化多语言 |
| <a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">Maxscale</a>  | Mariadb开发的一款成熟、高性能、免费开源的数据库中间件 |
| <a target="_blank" href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">AspNetCore.HealthChecks</a> | 健康监测组件,搭配consul的健康监测 |

## 后端解决方案
#### 整体架构图

- `Gateways` 网关相关工程

- `Infrastructures` 基础架构相关工程
- `Services` 微服务相关工程
- `Tests` 框架测试相关工程

![.NET微服务开源框架-整体架构图](https://aspdotnetcore.net/wp-content/uploads/2021/11/adnc_solution.jpg)

### Gateways网关相关工程

##### :white_check_mark: Adnc.Gateway.Ocelot 

基于Ocelot实现的Api网关，如果项目采用整体结构开发，该项目可以直接删除。Ocelot网关包含路由、服务聚合、服务发现、认证、鉴权、限流、熔断、缓存、Header头传递等功能。市面上主流网关还有Kong，Traefik，Ambassador，Tyk等等。网关和实际业务有没有关联，可以自由选择。

#### Infrastructures 基础架构相关工程
##### :white_check_mark: Adnc.Infra.Core
该工程是Adnc所有工程的最顶层，任何工程都会者直接或间接依赖该层。该工程提供了大量的`C#`基础类型的扩展方法以及Configuration、DependencyInjection、ContainerBuilder的扩展方法，还定义了一些异常类。

##### :white_check_mark: Adnc.Infra.Caching
该工程集成了StackExchange.Redis，提供缓存的管理、Redis常用类型操作、分布式锁、布隆过滤器功能。

##### :white_check_mark: Adnc.Infra.Consul
该工程集成了Consul，提供服务的自动注册、发现以及系统配置读写操作。

##### :white_check_mark: Adnc.Infra.EventBus
该工程简单封装了CAP，同时集成了RabbitMq，封装了发布者与订阅者等公共类，能让开发人员更加便捷的调用。

##### :white_check_mark: Adnc.Infra.IdGenerater
该工程负责Id的生成。

##### :white_check_mark: Adnc.Infra.Helper
该工程提供一些通用帮助类，如HashHelper,SecurityHelper等等。

##### :white_check_mark: Adnc.Infra.Mapper
该工程定义了IObjectMapper接口，并且提供了AutoMapper的实现。

##### :white_check_mark: Adnc.Infra.Repository
该工程定义了Entity对象的基类、UnitOfWork接口、仓储接口。

##### :white_check_mark: Adnc.Infra.EfCore.MySQL

该工程负责Adnc.Infra.Repository仓储接口`IAdoExecuterWithQuerierRepository`的实现，提供mysql数据库的CURD操作。

##### :white_check_mark: Adnc.Infra.Dapper

该工程负责Adnc.Infra.Repository仓储接口以及IUintofWork接口的EfCore的实现，提供mysql数据库的CURD操作，同时也集成了Dapper部分接口，用来处理复杂查询。

##### :white_check_mark: Adnc.Infra.Mongo

该工程负责Adnc.Infra.Repository仓储接口的Mongodb实现，提供mongodb数据库的CURD操作。

#### Services 微服务相关工程
该目录都是具体微服务业务的实现。
##### :white_check_mark: Shared 
微服务公用工程
- `Adnc.Shared` 该层目前只有两个目录Consts存放常量定义文件、Events存放事件定义文件。
- `Adnc.Shared.Rpc`该层负责proto文件的定义、refit接口定义以及DelegatingHandler实现。微服务同步通讯主要由该层处理，adnc支持Grpc与RestAPI两种通讯方式混用。
- `Adnc.Application.Shared` 该层定义了DTO对象的基类、应用服务类基类、缓存相关服务基类以及操作日志拦截器、UnitOfWork拦截器。所有微服务Application层的共享层，并且都需要依赖该层。
- `Adnc.WebApi.Shared` 该层实现了认证、鉴权、异常捕获、服务组件注册等公共类和中间件。所有微服务WebApi层的共享层，并且都需要依赖该层。
##### :white_check_mark: Adnc.Usr 
用户中心微服务，系统支撑服务，实现了用户管理、角色管理、权限管理、菜单管理、组织架构管理。
##### :white_check_mark: Adnc.Maint
运维中心微服务，系统支撑服务，实现了登录日志、审计日志、异常日志、字典管理、配置参数管理。
##### :white_check_mark: Adnc.Cus
客户中心微服务，经典三层开发模式demo。
##### :white_check_mark: Adnc.Ord
订单中心微服务，DDD开发模式demo。
##### :white_check_mark: Adnc.Whse
仓储中心微服务，DDD开发模式demo。

> 每个微服务的Migrations层是Efcore用来做数据迁移的，迁移的日志文件存放在各自Migrations目录中。

## 贡献者

<a href="https://github.com/alphayu/adnc/graphs/contributors">
  <img src="https://contributors-img.web.app/image?repo=alphayu/adnc" />
</a>

## License

**MIT**   
**Free Software, Hell Yeah!**
