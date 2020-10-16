# Adnc
[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/AlphaYu/Adnc)  
Adnc是一个基于dotnetcore前后端分离的轻量级微服务(microservices)快速开发框架,同时也可以应用于单体架构系统的开发。框架基于JWT认证授权，包含基础的后台管理功能，代码简洁、易上手、学习成本低、开箱即用。MIT License Free Software, Hell Yeah!   
- [演示地址](http://193.112.75.77)  alpha2008/alpha2008 
- [接口地址](http://193.112.75.77:8888/sys/index.html)
## 代码结构
  - clientApp 前端项目(vue)
  - serverApi 后端项目(dotnetcore)
  - doc 项目相关文档(数据库脚本/docker-compose.yaml文件)
  - tools 工具软件  
![image](http://193.112.75.77/adncimages/20201016154218.png)
##### ClientApp
  - ClientApp基于Vue-element-adminy以及web-flash(https://github.com/PanJiaChen/vue-element-admin) 搭建，感谢两位作者。
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
- 界面
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
| AspNetCore.HealthChecks | 健康检测插件 |
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


## 项目文件
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

#### 04.Adnc.Common
该层定义了一些公用模型与常量以及一些通用帮助类。该层不依赖任何层。
![image](http://193.112.75.77/adncimages/20201016160550.png)

License 
----
**MIT**   
**Free Software, Hell Yeah!**
