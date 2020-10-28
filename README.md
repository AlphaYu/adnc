![](https://discourse-cloud-file-uploads.s3.dualstack.us-west-2.amazonaws.com/github/original/2X/c/c0e0f8a6eae69b57a7465cdc578fc63874783f8d.png)
## 前言
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/AlphaYu/Adnc)
[![GitHub issues](https://img.shields.io/github/issues/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/issues)
[![GitHub forks](https://img.shields.io/github/forks/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/network)
[![GitHub stars](https://img.shields.io/github/stars/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/stargazers)
[![GitHub license](https://img.shields.io/github/license/AlphaYu/Adnc)](https://github.com/AlphaYu/Adnc/blob/master/LICENSE)<br/>
&ensp;&ensp;&ensp;&ensp;Adnc是一个轻量级的.Net Core 微服务(microservices)快速开发框架，同时也可以应用于单体架构系统的开发。框架基于JWT认证授权、集成了微服务相关配套组件，代码简洁、易上手、学习成本低、开箱即用。<br/>
&ensp;&ensp;&ensp;&ensp;Adnc前端基于Vue、后端服务基于.Net Core 3.1搭建，也是一个前后端分离的框架。WebApi遵循RESTful风格，框架包含用户/角色/权限管理、字典/配置管理、登录/审计/异常日志管理等基础的后台模块。<br/>
&ensp;&ensp;&ensp;&ensp;Adnc对配置中心、依赖注入、日志、缓存、模型映射、认证/授权、仓储、服务注册/发现、健康检测、性能检测、RabbitMq、EfCore、Dapper等模块进行更高一级的自动化封装，使Asp.Net Core 框架更易于应用到实际项目开发中。<br/>
> 演示网址：<a href="http://193.112.75.77" target="_blank">http://193.112.75.77</a> <br/>
> 账号/密码：alpha2008/alpha2008 
## 如何快速跑起来
  - [请点击链接，查看详细介绍](https://github.com/AlphaYu/Adnc/wiki/%E5%A6%82%E4%BD%95%E5%BF%AB%E9%80%9F%E8%B7%91%E8%B5%B7%E6%9D%A5)
## 目录结构
  - clientApp 前端项目(vue)
  - serverApi 后端项目(dotnetcore)
  - doc 项目相关文档(数据库脚本/docker-compose.yaml文件)
  - tools 工具软件  
![image](http://193.112.75.77/adncimages/20201016154218.png)
##### ClientApp
  - ClientApp基于Vue-element-admin以及web-flash搭建，感谢两位作者。
  - 技术栈 Vue + Vue-Router + Vuex + Axios
  - 构建步骤
    ```bash 
    # Install dependencies 
    npm install --registry=https://registry.npm.taobao.org
    # Serve with hot reload at localhost:5001
    npm run dev
    # Build for production with minification
    npm run build:prod
    ```
- 界面<br/>
![image](http://193.112.75.77/adncimages/20201016160306.png)
![image](http://193.112.75.77/adncimages/20201016160347.png)

##### ServerApi
  - ServerApi基于dotnetcore3.1搭建。
  - 主要技术栈
 
| 名称 | 描述 |
| ---- | -----|
| Ocelot | 基于 dotnetcore 编写的开源Api网关  |
| Consul | 配置中心、服务发现/注册组件|
| Refit  | 一个声明式自动类型安全的restful服务调用组件|
| SkyAPM.Agent.AspNetCore | skywalking .net core 探针，性能检测组件 |
| AspNetCore.HealthChecks | 健康检测组件 |
| Autofac | IOC容器组件 |
| Autofac.Extras.DynamicProxy | Autfac AOP扩展 |
| Efcore | ORM组件 |
| Dapper | 轻量级ORM组件 |
| Z.EntityFramework.Plus.EFCore | 高新能的EfCore组件 |
| NLog | 日志记录组件件 |
| AutoMapper | 模型映射组件 |
| Swashbuckle.AspNetCore | REST APIs文档生成工具（swagger） |
| EasyCaching | 实现一、二级缓存管理的一个开源的组件 |
| CAP  | 实现事件总线及最终一致性（分布式事务）的一个开源的组件 |
| RabbitMq  | 异步消息队列组件 |


## 项目介绍
### Adnc.Infras 基础架构层
##### 01.Adnc.WebApi.Shared
该层定义认证、鉴权、异常捕获等公共类于中间件。所有微服务WebApi层的共享层，都需要依赖该层。   
![image](http://193.112.75.77/adncimages/20201016160419.png)
##### 02.Adnc.Application.Shared  
该层定义了DTO对象的基类、应用服务类基类以及操作日志拦截器。所有微服务Application层的共享层，都需要依赖该层。   
![image](http://193.112.75.77/adncimages/20201016160452.png)
##### 03.Adnc.Core.Shared
该层定义了Entity对象的基类、业务服务接口基类、Rpc服务通用服务、UOW接口与拦截器以及仓储接口。所有微服务Core层的共享层，都需要依赖该层。     
![image](http://193.112.75.77/adncimages/20201016160512.png)
##### 04.Adnc.Common
该层定义了一些公用模型与常量以及一些通用帮助类。该层不依赖任何层。
![image](http://193.112.75.77/adncimages/20201016160550.png)
##### 10.Adnc.Infr.Gateway 
 该层是一个输出项目，Api网关，基于ocelot实现，如果项目采用整体结构开发，该项目可以直接删除。ocelot网关包含路由、服务聚合、服务发现、认证、鉴权、限流、熔断、缓存、Header头传递等功能。市面上主流网关还有Kong，Traefik，Ambassador，Tyk等。<br/>
![image](http://193.112.75.77/adncimages/20201017111155.png)
##### 11.Adnc.Infr.HealthCheckUI
该层是一个输出项目， AspNetCore.HealthChecks组件的Dashboard，直接配置需要监测的服务地址就可以了，没有代码。
##### 20.Adnc.Infr.Consul
该层集成了Consul。提供服务的注册、发现以及系统配置读取等公共类。
![image](http://193.112.75.77/adncimages/20201017115934.png)
##### 21.Adnc.Infr.EasyCaching
该层集成了EasyCaching。重写了EasyCaching拦截器部分代码，负责一、二级缓存的管理。
![image](http://193.112.75.77/adncimages/20201017120053.png)
##### 22.Adnc.Infr.EfCore
该层负责Adnc.Core.Shared仓储接口与Uow的Ef实现，负责mysql数据库的操作。同时也集成了Dapper部分接口，用来处理复杂查询。<br/>
![image](http://193.112.75.77/adncimages/20201017120005.png)
##### 23.Adnc.Infr.Mongo
该层负责Adnc.Core.Shared仓储接口的Mongodb实现，负责mongodb数据库的操作。
![image](http://193.112.75.77/adncimages/20201017120115.png)
##### 23.Adnc.Infr.RabbitMq
该层集成了RabbitMq。封装了发布者与订阅者等公共类。<br/>
![image](http://193.112.75.77/adncimages/20201017120028.png)
## 代码片段
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
## 问题交流
-  企&ensp;鹅&ensp;群：780634162
-  博&ensp;&ensp;&ensp;&ensp;客：https://www.cnblogs.com/alphayu
-  github&ensp; ：https://github.com/alphayu
-  项目网址：https://www.aspdotnetcore.net
## License 
**MIT**   
**Free Software, Hell Yeah!**
