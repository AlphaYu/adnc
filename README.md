[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/AlphaYu/Adnc)
[![GitHub license](https://img.shields.io/github/license/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/blob/master/LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/issues)
[![GitHub stars](https://img.shields.io/github/stars/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/network)
# Adnc
&ensp;&ensp;&ensp;&ensp;Adnc是一个轻量级的`.Net Core`微服务快速开发框架，同时也可以应用于单体架构系统的开发。框架基于JWT认证授权、集成了微服务相关配套组件，代码简洁、易上手、学习成本低、开箱即用。<br/>
&ensp;&ensp;&ensp;&ensp;前端基于`Vue`、后端服务基于`.Net Core 3.1`搭建，也是一个前后端分离的框架。webapi遵循RESTful风格，框架包含用户、角色、权限、部门管理；字典、配置管理；登录、审计、异常日志管理等基础的后台模块。<br/>
&ensp;&ensp;&ensp;&ensp;框架对配置中心、依赖注入、日志、缓存、模型映射、认证/授权、仓储、服务注册/发现、健康检测、性能与链路监测、队列、ORM、EventBus等模块进行更高一级的自动化封装，更易于开发`Asp.NET Core`微服务项目。<br/>
## **演示**
- <a href="http://193.112.75.77" target="_blank">http://193.112.75.77</a>
## **相关文档**
### **如何快速跑起来**
- 详细介绍如何使用docker安装reids、mysql、rabbitmq、mongodb，以及如何在本地配置ClientApp、ServerApi。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/%E5%A6%82%E4%BD%95%E5%BF%AB%E9%80%9F%E8%B7%91%E8%B5%B7%E6%9D%A5)

### **如何手动速部署到服务器**
- 详细介绍如何使用docker安装consul集群、使用consul注册中心、安装配置Skywalking，以及相关项目dockfile文件编写和配置等。<br/>
[请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/Adnc%E5%A6%82%E4%BD%95%E6%89%8B%E5%8A%A8%E9%83%A8%E7%BD%B2(docker,consul,skywalking,nginx))

## **目录结构**
  - ClientApp 前端项目(`Vue`)
  - ServerApi 后端项目(`.NET Core 3.1`)
  - Doc 项目相关文档(sql脚本、docker脚本、docker-compose.yaml文件)
  - Tools 工具软件  
### **ClientApp**
  - ClientApp基于Vue-Element-Admin以及Web-Flash搭建，感谢两位作者。
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
#### **界面**
![.NET Core微服务开源框架-异常日志界面](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-dashboard-nlog.png)
![.NET Core微服务开源框架-角色管理界面](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-dashboard-role.png)

### **ServerApi**
  - ServerApi基于`.NET CORE 3.1`搭建。
  - 后端主要技术栈
  
| 名称 | 描述 |
| ---- | -----|
| Ocelot | 基于 `.NET Core` 编写的开源Api网关  |
| Consul | 配置中心、注册中心组件|
| Refit  | 一个声明式自动类型安全的RESTful服务调用组件，用于同步调用其他微服务|
| SkyAPM.Agent.AspNetCore | Skywalking `.NET Core`探针，性能链路监测组件 |
| AspNetCore.HealthChecks | 健康监测组件,搭配consul的健康监测 |
| Autofac | IOC容器组件 |
| Autofac.Extras.DynamicProxy | Autfac AOP扩展 |
| Efcore | 微软的ORM组件 |
| Dapper | 轻量级ORM组件 |
| Z.EntityFramework.Plus.EFCore | 第三方高性能的EfCore组件 |
| NLog | 日志记录组件 |
| AutoMapper | 模型映射组件 |
| Swashbuckle.AspNetCore | APIs文档生成工具(swagger) |
| EasyCaching | 实现了一、二级缓存管理的一个开源的组件 |
| CAP  | 实现事件总线及最终一致性（分布式事务）的一个开源的组件 |
| RabbitMq  | 异步消息队列组件 |
| Polly  | 一个 .NET 弹性和瞬态故障处理库，允许开发人员以 Fluent 和线程安全的方式来实现重试、断路、超时、隔离和回退策略 |

## 后端解决方案
#### **整体架构图**
`Adnc.Infras` 基础架构相关工程<br/>
`Adnc.Portal` 微服务相关工程<br/>
![.NET Core微服务开源框架-整体架构图](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-structure.png)
#### **Adnc.Infras 基础架构相关工程**
##### 01.Adnc.WebApi.Shared
该层实现了认证、鉴权、异常捕获等公共类和中间件。所有微服务WebApi层的共享层，并且都需要依赖该层。<br/>
![.NET Core微服务开源框架-webpai-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-webapishared.png)
##### 02.Adnc.Application.Shared  
该层定义了DTO对象的基类、Rpc服务通用服务、应用服务类基类以及操作日志拦截器。所有微服务Application层的共享层，并且都需要依赖该层。<br/>
![.NET Core微服务开源框架-application-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-applicationshared.png)
##### 03.Adnc.Core.Shared
该层定义了Entity对象的基类、业务服务接口基类、UOW接口与拦截器、仓储接口、以及处理本地事务与分布式事务。所有微服务Core层的共享层，并且都需要依赖该层。<br/>
![.NET Core微服务开源框架-core-shared层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-coreshared.png)
##### 04.Adnc.Infr.Common
该层实现了一些通用帮助类。该层不依赖任何层。<br/>
![.NET Core微服务开源框架-基础机构-common层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-common.png)
##### 10.Adnc.Infr.Gateway 
 该层是一个输出项目，基于Ocelot实现的Api网关，如果项目采用整体结构开发，该项目可以直接删除。ocelot网关包含路由、服务聚合、服务发现、认证、鉴权、限流、熔断、缓存、Header头传递等功能。市面上主流网关还有Kong，Traefik，Ambassador，Tyk等。<br/>
![.NET Core微服务开源框架-基础机构-gateway层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-gateway.png)
##### 11.Adnc.Infr.HealthCheckUI
该层是一个输出项目， AspNetCore.HealthChecks组件的Dashboard，直接配置需要监测的服务地址就可以了，没有代码，关键的代码参考webapi层的AddHealthChecks()方法。<br/>
![.NET Core微服务开源框架-基础机构-healthchecksui层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-healthcheckui.png)
##### 20.Adnc.Infr.Consul
该层集成了Consul，提供服务的自动注册、发现以及系统配置读写。<br/>
![.NET Core微服务开源框架-基础机构-cosnul层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-consul.png)
##### 21.Adnc.Infr.EasyCaching
该层集成了EasyCaching，负责一、二级缓存的管理，并重写了EasyCaching拦截器部分代码。<br/>
![.NET Core微服务开源框架-基础机构-easycaching层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-easycaching.png)
##### 22.Adnc.Infr.EfCore
该层负责Adnc.Core.Shared仓储接口与Uow的EfCore的实现，负责mysql数据库的操作。同时也集成了Dapper部分接口，用来处理复杂查询。<br/>
![.NET Core微服务开源框架-基础机构-efcore层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-efcore.png)
##### 23.Adnc.Infr.Mongo
该层负责Adnc.Core.Shared仓储接口的Mongodb实现，负责mongodb数据库的操作。<br/>
![.NET Core微服务开源框架-基础机构-mongodb层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-mongodb.png)
##### 23.Adnc.Infr.RabbitMq
该层集成了RabbitMq。封装了发布者与订阅者等公共类，方便更加便捷的调用rabbitmq。<br/>
![.NET Core微服务开源框架-基础机构-rabbitmq层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-infr-rabbitmq.png)
#### **Adnc.Portal 微服务相关工程**
该层都是具体微服务业务的实现。<br/>
`Adnc.Usr` 用户中心微服务，实现了用户、角色、权限、部门管理。<br/>
`Adnc.Maint` 运维中心微服务，实现了登录、审计、异常日志管理以及一些配套组件的外链。<br/>
`Adnc.Cus` 客户中心微服务，该层主要是一些demo。<br/>
每个微服务的Migrations层是Efcore用来做数据迁移的，迁移的日志文件存放在各自Migrations目录中。<br/>
![.NET Core微服务开源框架-微服务层](https://aspdotnetcore.net/wp-content/uploads/2020/11/adnc-serverapi-potral.png)
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
## **下一步计划**
  - 完善框架文档。
  - 集成<a href="https://github.com/ldqk/Masuit.Tools" target="_blank">Masuit.Tools</a>丰富Adnc.Infr.Common工具层。
  - 集成<a href="https://github.com/quartznet/quartznet" target="_blank">Quartz.Net</a>实现框架计划调度功能。
## **问题交流**
-  企&ensp;鹅&ensp;群：780634162
-  博&ensp;&ensp;&ensp;&ensp;客：https://www.cnblogs.com/alphayu
-  GitHub&ensp; ：https://github.com/alphayu/adnc
-  项目网址：https://aspdotnetcore.net
## **License**
**MIT**   
**Free Software, Hell Yeah!**
