# ADNC 服务层开发指引

[GitHub 仓库地址](https://github.com/alphayu/adnc)

服务层（`Application` / `Application.Contracts`）负责业务编排、DTO 映射、事务控制、跨服务调用等，是应用层的核心组成部分。建议遵循分层、解耦、可测试的设计原则。

---

## 1. 设计原则

- 单一职责：每个应用服务只处理一个聚合/模块的业务。
- 接口与实现分离：接口定义于 `Application.Contracts`，实现放在 `Application`。

## 2. 目录结构

```
Application/
├── Services/         # 业务服务实现
├── Dtos/             # 数据传输对象
├── Validators/       # 参数校验
├── Cache/            # 缓存处理对象
├── Subscribers/      # 订阅者对象(如果有)
├── MapperProfile.cs       # 对象映射配置
└── DependencyRegistrar.cs # 依赖注册
Contracts/
├── Dtos/             # DTO 定义
└── Interfaces/       # 接口定义
```

## 3. 依赖注入与拦截器

- 通过构造函数注入仓储、远程服务、领域服务等依赖，避免 Service Locator 等隐式依赖写法。
- 框架会自动应用缓存、日志、事务等拦截器。

## 4. DTO 映射

- 推荐使用 Mapster/AutoMapper 完成实体与 DTO 的转换。
- DTO 仅用于数据承载，不应包含业务逻辑。

## 5. 事务控制

- 支持自动事务（拦截器）或手动控制（`IUnitOfWork`）。
- 建议将事务边界控制在服务层。

## 6. 跨服务调用

- 支持 Refit/gRPC/CAP 事件等多种远程调用方式。
- 远程接口定义于 `Shared/Remote.Http`、`Remote.Grpc`、`Remote.Event`。

## 7. 参数校验

- 使用 FluentValidation 定义参数校验规则。
- 框架会自动注册并应用校验器。

## 8. 错误与异常

- 异常中间件处理。
- 返回标准 `ServiceResult` / `Problem`。

## 9. 参考文档

- [服务层开发文档](https://aspdotnetcore.net/docs/application-service/)
- 详见 Demo 各服务 Application 层实现
