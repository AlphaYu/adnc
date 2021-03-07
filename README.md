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

# <div align="center">![Adnc是一个微服务开发框架 代码改变世界 开源活跃社区](https://aspdotnetcore.net/wp-content/uploads/2020/12/adnc-homepage-logo-3.webp)</div>
&ensp;&ensp;&ensp;&ensp;<a target="_blank" title="一个轻量级的.Net Core微服务开发框架" href="https://aspdotnetcore.net">Adnc</a>是一个轻量级的<a target="_blank" href="https://github.com/dotnet/core">.Net Core</a>微服务开发框架，同时也适用于单体架构系统的开发。支持经典三层与DDD架构开发模式、集成了一系列主流稳定的微服务配套技术栈。一个前后端分离的框架，前端基于<a target="_blank" href="https://github.com/vuejs/vue">Vue</a>、后端基于<a target="_blank" href="https://github.com/dotnet/core">.Net Core 3.1</a>构建。Webapi遵循RESTful设计规范、基于JWT认证授权、基于<a target="_blank" href="https://github.com/mariadb-corporation/MaxScalehttps://github.com/mariadb-corporation/MaxScale">Maxscale</a>实现了读写分离、部署灵活、代码简洁、开箱即用、容器化微服务的最佳实践。<br/>

<table style="font-size:13px;">
    <tr>
        <th colspan="4">Adnc包含的微服务介绍</th>
    </tr>
    <tr>
        <td width="40px">Adnc.Usr</td>
        <td width="100px">用户中心</td>
        <td width="100px"  rowspan="3">经典三层</td>        
        <td>系统支撑服务，实现了用户管理、角色管理、权限管理、菜单管理、组织架构管理。</td>
    </tr>
    <tr>
        <td>Adnc.Maint</td>
        <td>运维中心</td>    
        <td>系统支撑服务，实现了登录日志、审计日志、异常日志、字典管理、配置参数管理。</td>
    </tr>
    <tr>
        <td>Adnc.Cust</td>
        <td>客户中心</td> 
        <td rowspan="3">三个demos，完整的演示了如何使用服务注册/发现、配置中心、仓储、Refit调用微服务、异步消息队列、EventBus、领域事件的发布订阅、领域服务/聚合根/实体/值对象设计、组件依赖注入、异常拦截、日志拦截、缓存配置、DTO参数校验、RESTful规范的API设计、模型映射、工作单元、健康检测配置、性能与链路监测配置等。
        </td>
    </tr>
    </tr>
        <tr>
        <td>Adnc.Ord</td>
        <td>订单中心</td>
        <td  rowspan="2">DDD架构</td>   
    </tr>
    </tr>
        <tr>
        <td>Adnc.Whse</td>
        <td>仓储中心</td>
    </tr>
</table>

## 演示
- <a href="http://adnc.aspdotnetcore.net" target="_blank">http://adnc.aspdotnetcore.net</a>

## GitHub
- <a href="https://github.com/alphayu/adnc" target="_blank">https://github.com/alphayu/adnc</a>
- 开源不易，如果您喜欢这个项目, 请给个星星⭐️。

## 文档
#### 如何快速跑起来
- 详细介绍如何使用docker安装reids、mysql、rabbitmq、mongodb，以及如何在本地配置ClientApp、ServerApi。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/%E5%A6%82%E4%BD%95%E5%BF%AB%E9%80%9F%E8%B7%91%E8%B5%B7%E6%9D%A5)

#### 如何手动部署到服务器
- 详细介绍如何使用docker安装consul集群、使用consul注册中心、安装配置Skywalking，以及相关项目dockerfile文件编写和配置等。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/Adnc%E5%A6%82%E4%BD%95%E6%89%8B%E5%8A%A8%E9%83%A8%E7%BD%B2(docker,consul,skywalking,nginx))

#### 如何实现读写分离
- 详细介绍为什么要通过中间件实现读写分离以及EFCore基于中间件如何写代码。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/%E5%A6%82%E4%BD%95%E5%AE%9E%E7%8E%B0%E8%AF%BB%E5%86%99%E5%88%86%E7%A6%BB)

#### 如何使用EFCore仓储
- 详细介绍`EFCore`仓储基础功能与工作单元的使用，提供了丰富的演示代码以及演示代码对应的Sql语句。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/%E5%A6%82%E4%BD%95%E4%BD%BF%E7%94%A8%E4%BB%93%E5%82%A8(%E4%B8%80)-%E5%9F%BA%E7%A1%80%E5%8A%9F%E8%83%BD)

## 目录结构
  - ClientApp 前端项目(`Vue`)
  - ServerApi 后端项目(`.NET Core 3.1`)
  - Doc 项目相关文档(sql脚本、docker脚本、docker-compose.yaml文件)
  - Tools 工具软件  
  - Test 测试工程
#### ClientApp
  - ClientApp基于<a target="_blank" href="https://github.com/PanJiaChen/vue-element-admin">Vue-Element-Admin</a>以及<a target="_blank" href="https://github.com/enilu/web-flash">Web-Flash</a>搭建，感谢两位作者。
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
![.NET微服务开源框架-异常日志界面](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-dashboard-nlog.webp)
![.NET微服务开源框架-角色管理界面](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-dashboard-role.webp)

#### ServerApi
  - ServerApi基于`.NET CORE 3.1`搭建。
  - 后端主要技术栈
  
| 名称 | 描述 |
| ---- | -----|
| <a target="_blank" href="https://github.com/ThreeMammals/Ocelot">Ocelot</a> | 基于 `.NET Core` 编写的开源网关  |
| <a target="_blank" href="https://github.com/hashicorp/consul">Consul</a> | 配置中心、注册中心组件|
| <a target="_blank" href="https://github.com/reactiveui/refit">Refit</a>  | 一个声明式自动类型安全的RESTful服务调用组件，用于同步调用其他微服务|
| <a target="_blank" href="https://github.com/SkyAPM/SkyAPM-dotnet">SkyAPM.Agent.AspNetCore</a> | Skywalking `.NET Core`探针，性能链路监测组件 |
| <a target="_blank" href="https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks">AspNetCore.HealthChecks</a> | 健康监测组件,搭配consul的健康监测 |
| <a target="_blank" href="https://github.com/autofac/Autofac">Autofac</a> | IOC容器组件 |
| <a target="_blank" href="https://github.com/autofac/Autofac">Autofac.Extras.DynamicProxy</a> | Autfac AOP扩展 |
| <a target="_blank" href="https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql">Pomelo.EntityFrameworkCore.MySql</a> | EFCore ORM组件 |
| <a target="_blank" href="https://github.com/StackExchange/Dapper">Dapper</a> | 轻量级ORM组件 |
| <a target="_blank" href="https://entityframework-plus.net">Z.EntityFramework.Plus.EFCore</a> | 第三方高性能的EfCore组件 |
| <a target="_blank" href="https://github.com/NLog/NLog">NLog</a> | 日志记录组件 |
| <a target="_blank" href="https://github.com/AutoMapper/AutoMapper">AutoMapper</a> | 模型映射组件 |
| <a target="_blank" href="https://github.com/domaindrivendev/Swashbuckle.AspNetCore">Swashbuckle.AspNetCore</a> | APIs文档生成工具(swagger) |
| <a target="_blank" href="https://github.com/dotnetcore/EasyCaching">EasyCaching</a> | 实现了一、二级缓存管理的一个开源的组件 |
| <a target="_blank" href="https://github.com/dotnetcore/CAP">CAP</a>  | 实现事件总线及最终一致性（分布式事务）的一个开源的组件 |
| <a target="_blank" href="https://github.com/rabbitmq/rabbitmq-dotnet-client">RabbitMq</a>  | 异步消息队列组件 |
| <a target="_blank" href="https://github.com/App-vNext/Polly">Polly</a>  | 一个 .NET 弹性和瞬态故障处理库，允许开发人员以 Fluent 和线程安全的方式来实现重试、断路、超时、隔离和回退策略 |
| <a target="_blank" href="https://github.com/FluentValidation">FluentValidation</a>  | 一个 .NET 验证框架，支持链式操作，易于理解，功能完善，组件内提供十几种常用验证器，可扩展性好，支持自定义验证器，支持本地化多语言 |
| <a target="_blank" href="https://github.com/mariadb-corporation/MaxScale">Maxscale</a>  | Mariadb开发的一款成熟、高性能、免费开源的数据库中间件 |

## 后端解决方案
#### 整体架构图
`Adnc.Infras` 基础架构相关工程<br/>
`Adnc.Portal` 微服务相关工程<br/>
![.NET微服务开源框架-整体架构图](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-structure.webp)
#### Adnc.Infras 基础架构相关工程
##### 01.Adnc.WebApi.Shared
该层实现了认证、鉴权、异常捕获等公共类和中间件。所有微服务WebApi层的共享层，并且都需要依赖该层。<br/>
![.NET微服务开源框架-webpai-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-webapishared.webp)
##### 02.Adnc.Application.Shared  
该层定义了DTO对象的基类、Rpc服务通用服务、应用服务类基类以及操作日志拦截器。所有微服务Application层的共享层，并且都需要依赖该层。<br/>
![.NET微服务开源框架-application-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-applicationshared.webp)
##### 03.Adnc.Core.Shared
该层定义了Entity对象的基类、业务服务接口基类、UOW接口与拦截器、仓储接口、以及处理本地事务与分布式事务。所有微服务Core层的共享层，并且都需要依赖该层。<br/>
![.NET微服务开源框架-core-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-coreshared.webp)
##### 04.Adnc.Infr.Common
该层实现了一些通用帮助类。该层不依赖任何层。<br/>
![.NET微服务开源框架-基础机构-common层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-common.webp)
##### 10.Adnc.Infr.Gateway 
 该层是一个输出项目，基于Ocelot实现的Api网关，如果项目采用整体结构开发，该项目可以直接删除。ocelot网关包含路由、服务聚合、服务发现、认证、鉴权、限流、熔断、缓存、Header头传递等功能。市面上主流网关还有Kong，Traefik，Ambassador，Tyk等。<br/>
![.NET微服务开源框架-基础机构-gateway层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-gateway.webp)
##### 11.Adnc.Infr.HealthCheckUI
该层是一个输出项目， AspNetCore.HealthChecks组件的Dashboard，直接配置需要监测的服务地址就可以了，没有代码，关键的代码参考webapi层的AddHealthChecks()方法。<br/>
![.NET微服务开源框架-基础机构-healthchecksui层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-healthcheckui.webp)
##### 20.Adnc.Infr.Consul
该层集成了Consul，提供服务的自动注册、发现以及系统配置读写。<br/>
![.NET微服务开源框架-基础机构-cosnul层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-consul.webp)
##### 21.Adnc.Infr.EasyCaching
该层集成了EasyCaching，负责一、二级缓存的管理，并重写了EasyCaching拦截器部分代码。<br/>
![.NET微服务开源框架-基础机构-easycaching层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-easycaching.webp)
##### 22.Adnc.Infr.EfCore
该层负责Adnc.Core.Shared仓储接口与Uow的EfCore的实现，负责mysql数据库的操作。同时也集成了Dapper部分接口，用来处理复杂查询。<br/>
![.NET微服务开源框架-基础机构-efcore层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-efcore.webp)
##### 23.Adnc.Infr.Mongo
该层负责Adnc.Core.Shared仓储接口的Mongodb实现，负责mongodb数据库的操作。<br/>
![.NET微服务开源框架-基础机构-mongodb层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-mongodb.webp)
##### 23.Adnc.Infr.RabbitMq
该层集成了RabbitMq。封装了发布者与订阅者等公共类，方便更加便捷的调用rabbitmq。<br/>
![.NET微服务开源框架-基础机构-rabbitmq层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-rabbitmq.webp)
#### Adnc.Portal 微服务相关工程
该层都是具体微服务业务的实现。<br/>
`Adnc.Usr` 用户中心微服务，实现了用户、角色、权限、部门管理。<br/>
`Adnc.Maint` 运维中心微服务，实现了登录、审计、异常日志管理以及一些配套组件的外链。<br/>
`Adnc.Cus` 客户中心微服务，该层主要是一些demo。<br/>
每个微服务的Migrations层是Efcore用来做数据迁移的，迁移的日志文件存放在各自Migrations目录中。<br/>
![.NET微服务开源框架-微服务层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-potral.webp)
### 代码片段
```csharp
    [Route("usr/session")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JWTConfig _jwtConfig;
        private readonly IAccountAppService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IOptionsSnapshot<JWTConfig> jwtConfig
            , IAccountAppService accountService
            , ILogger<AccountController> logger)
        {
            _jwtConfig = jwtConfig.Value;
            _accountService = accountService;
            _logger = logger;
        }

        /// <summary>
        /// 登录/验证
        /// </summary>
        /// <param name="userDto"><see cref="UserValidateInputDto"/></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost()]
        public async Task<UserTokenInfoDto> Login([FromBody]UserValidateInputDto userDto)
        {
            var userValidateDto = await _accountService.Login(userDto);

            return new UserTokenInfoDto
            {
                Token = JwtTokenHelper.CreateAccessToken(_jwtConfig, userValidateDto),
                RefreshToken = JwtTokenHelper.CreateRefreshToken(_jwtConfig, userValidateDto)
            };
        }
    }
```

```csharp
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper;
        private readonly IEfRepository<SysUser> _userRepo;
        private readonly RabbitMqProducer _mqProducer;
        public AccountAppService(IMapper mapper,
            IEfRepository<SysUser> userRepo,
            RabbitMqProducer mqProducer)
        {
            _mapper = mapper;
            _userRepo = userRepo;
            _mqProducer = mqProducer;
        }

        public async Task<UserValidateDto> Login(UserValidateInputDto inputDto)
        {
            var user = await _userRepo.FetchAsync(x => new { x.Password, x.Salt, x.Name, x.Email, x.RoleId,x.Account,x.ID,x.Status }
            , x => x.Account == inputDto.Account);
            //todo......
            //..........
            _mqProducer.BasicPublish(MqConsts.Exchanges.Logs, MqConsts.RoutingKeys.Loginlog, log);
            return _mapper.Map<UserValidateDto>(user);
        }
    }
```
## 下一步计划
  - 完善框架文档。
  - 优化现有功能
  - 开发微服务项目生成工具
  - 集成<a href="https://github.com/quartznet/quartznet" target="_blank">Quartz.Net</a>实现框架计划调度功能。

## 问题交流
-  企&ensp;鹅&ensp;群：780634162
-  项目官网：<a target="_blank" href="https://aspdotnetcore.net">https://aspdotnetcore.net</a>
-  博&ensp;&ensp;&ensp;&ensp;客：<a target="_blank" href="https://www.cnblogs.com/alphayu">https://www.cnblogs.com/alphayu</a>
-  GitHub&ensp;：<a target="_blank" href="https://github.com/alphayu/adnc">https://github.com/alphayu/adnc</a>

## License
**MIT**   
**Free Software, Hell Yeah!**
