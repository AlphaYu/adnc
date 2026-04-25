# ADNC API 层开发指引

[GitHub 仓库地址](https://github.com/alphayu/adnc)

API 层负责对外提供 HTTP 接口，处理路由与协议适配、鉴权与权限校验、参数绑定与输入校验、响应封装与错误输出等。API 层应保持“薄”，避免承载业务规则与事务逻辑，将业务编排交由服务层（`Application`）完成。

---

## 1. 设计原则

- 职责清晰：API 层只做协议适配与边界控制（路由、鉴权、参数、响应），不直接编写业务规则。
- 面向契约：输入/输出仅使用 DTO（`Application.Contracts`），避免直接暴露实体模型。
- 一致性：统一的路由风格、响应结构、错误码/错误信息策略，便于前端与第三方集成。
- 可观测性：接口日志、链路追踪、审计信息应在框架层统一处理，业务代码不重复实现。

## 2. 目录结构（示例）

```
Api/
├── Controllers/              # 控制器
├── Filters/                  # 过滤器(可选)
├── Consts.cs                 # 常量/权限码(可选)
├── DependencyRegistrar.cs    # 依赖注册 
├── MiddlewareRegistrar.cs    # 中间件注册
├── Program.cs                # 应用入口
└── appsettings*.json         # 配置文件
```

## 3. 路由与控制器规范

- Controller 命名：以资源名命名（如 `StudentController`），路由采用复数资源（如 `/students`）。
- 路由组织：建议基于统一的路由根（例如 `RouteConsts.Admin`），避免散落硬编码。
- 控制器基类：统一继承框架提供的控制器基类（例如 `AdncControllerBase`），以复用通用返回结果与基础能力。

## 4. 鉴权与权限控制

- 默认鉴权：建议通过全局策略要求所有接口认证通过后访问；确需匿名接口使用显式标注（如 `[AllowAnonymous]`）。
- 权限最小化：对写接口（创建/更新/删除）与敏感读接口配置权限码；权限码建议集中管理（如 `PermissionConsts`）。
- 认证方案：在支持多认证方案的场景下，明确每个接口允许的认证方案组合，避免默认行为不清晰。

## 5. 参数绑定与输入校验

- 参数来源明确：`[FromRoute]`/`[FromQuery]`/`[FromBody]` 明确标注，避免隐式绑定导致歧义。
- 校验前置：DTO 输入校验使用 FluentValidation，并由框架统一触发；API 层不手写重复的 if/else 校验。
- 语义化 DTO：创建、更新、查询、分页等 DTO 分离（如 `CreationDto`/`UpdationDto`/`SearchPagedDto`），避免一个 DTO 承载多种语义。

## 6. 响应与错误处理

- 返回结构：优先返回统一的结果结构（如 `ServiceResult`/`Problem`），避免同一类接口出现多种返回格式。
- HTTP 语义：创建返回 201，更新/删除成功返回 204 或统一结果；资源不存在返回 404；校验失败返回 400。
- 异常边界：业务异常与系统异常由统一异常处理中间件转换为标准错误响应，API 层不直接吞异常。

## 7. 接口文档与示例

- OpenAPI/Swagger：为 Controller/Action 添加必要的摘要与返回码标注，保证生成文档可读。
- 示例清晰：对分页、批量操作、导入导出等接口给出请求示例与字段说明，降低使用门槛。

## 8. 参考实现

- 详见 Demo 各服务的 `Api/Controllers` 与对应的 `Application` 服务实现。

