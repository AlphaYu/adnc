# Adnc

[演示地址](http://193.112.75.77) 用户名/密码(alpha2008/alpha2008)

[接口地址](http://193.112.75.77:8888/sys/index.html)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/AlphaYu/Adnc)

Adnc 是一个基于asp.netcore前后端分离的基础开发框架(支持微服务)。

  - ClientApp 前端项目
  - ServerApi 后端项目
  - doc 项目相关文档(数据库脚本/docker-compose.yaml文件)

## ClientApp
  - ClientApp基于Vue-element-adminy以及web-flash(https://github.com/PanJiaChen/vue-element-admin) 搭建，感谢两位作者。
  - 技术栈 Vue + Vue-Router + Vuex + Axios
  - 构建步骤jieyu
    ```bash 
    # Install dependencies 
    npm install --registry=https://registry.npm.taobao.org
    # Serve with hot reload at localhost:5001
    npm run dev
    # Build for production with minification
    npm run build:prod
    ```
- 界面

## ServerApi
  - ServerApi基于.netcore3.1搭建。
  - 主要技术栈
 
| 名称 | 描述 |
| ------ | ------ |
| Ocelot | 基于 .net core 编写的开源Api网关  |
| Consul | 配置中心、服务发现/注册|
| SkyAPM.Agent.AspNetCore | skywalking .net core 探针，性能检测 |
| AspNetCore.HealthChecks | 健康检测插件 |
| Autofac | IOC容器 |
| Autofac.Extras.DynamicProxy | AOP |
| Efcore | Orm框架 |
| Z.EntityFramework.Plus.EFCore | 高新能的批量更新Ef插件 |
| NLog | 日志记录插件 |
| AutoMapper | 模型映射 |
| Swashbuckle.AspNetCore | REST APIs文档生成工具（swagger） |
| EasyCaching | 实现一、二级缓存管理的一个开源的 C# 库 |
| CAP  | 实现事件总线及最终一致性（分布式事务）的一个开源的 C# 库 |
| xUnit | 单元测试模板 |

  - 项目结构
00-09 输出类项目(webapi,mvc,gRpc,console)、10-19业务类项目、:20-29基础设施层、30-xx 测试项目.

| 项目名称 | 描述 |
| ------ | ------ |
| 01.Adnc.Gateway | Api网关，基于ocelot实现，如果项目采用整体结构开发，该项目可以直接删除。ocelot网关包含路由、服务聚合、服务发现、认证、鉴权、限流、熔断、缓存、Header头传递等功能。市面上主流网关还有Kong，Traefik，Ambassador，Tyk等 |
| 02.Adnc.WebApi | 系统接口，为前端项目提供服务。 (http://193.112.75.77:8888/sys/index.html)， 基于JWT认证，基本上遵循resetful设计风格。负责认证、授权、异常捕获 |
| 03.Adnc.Maintaining | 运维管理，这是一个单独的项目，目前只基集成了健康检测，后期想把性能监控也集成到这个项目 |
| 10.Adnc.Application | 应用层，协调webapi层与core层，负责dot<=>entity转换、cache管理(一、二级缓存)、记录日志、消息发送等 |
| 11.Adnc.Core | 核心业务层，包含实体、仓储(接口)、单库事务/分布式事务、EventBus。 |
| 19.Adnc.Common | 公共层，提供帮助类和扩展方法以及一些公用模型 |
| 20.Adnc.Infr.Consul | 基础设施层，集成了Consul。负责服务的注册、发现以及系统配置读取 |
| 21.Adnc.Infr.EasyCaching | 基础设施层，集成了EasyCaching。重写了EasyCaching拦截器部分代码，负责一、二级缓存的管理 |
| 22.Adnc.Infr.EfCore| 基础设施层，Adnc.Core仓储接口的Ef实现，负责mysql数据库的CRUD操作 |
| 23.Adnc.Infr.Mongo| 基础设施层，Adnc.Core仓储接口的MongoDb实现,负责MongoDb数据库的CRUD操作 |
| 30.Adnc.UnitTest| 单元测试，集成了xunit测试模板 |

License
----

MIT


**Free Software, Hell Yeah!**
