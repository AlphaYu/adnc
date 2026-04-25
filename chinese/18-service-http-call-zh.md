# ADNC 服务之间如何通过 HTTP 调用（Refit）

[GitHub 仓库地址](https://github.com/alphayu/adnc)

在 ADNC 中，服务间“同步调用”（A 服务去调用 B 服务的接口）通常使用 HTTP（基于 Refit）或 gRPC。本文以 `src/Demo/Cust/Api/Controllers/RestClientDemoController.cs` 为例，说明 Cust 服务如何通过 HTTP 调用 Admin 服务的接口，涵盖：接口定义、客户端注册、地址配置、鉴权方式与容错策略。

---

## 0. 快速上手（3 步）

1. 定义 Refit 接口（放在共享项目里）：例如 `src/Demo/Shared/Remote.Http/Services/IAdminRestClient.cs`。
2. 在调用方服务注册客户端：例如 `src/Demo/Cust/Api/DependencyRegistrar.cs` 中调用 `AddRestClient<IAdminRestClient>(...)`。
3. 在业务代码里注入并调用：例如 Controller/Service 通过构造函数注入 `IAdminRestClient`，然后调用接口方法。

## 1. 设计建议（写给使用者）

- 尽量用在“查询/校验”这类读操作：跨服务同步调用链越长，越容易放大延迟与故障面。涉及跨服务一致性时，优先考虑事件驱动（如 CAP）。
- 只依赖“接口 + DTO”：不要直接引用对方的 API 工程，也不要跨服务共享实体模型。
- 把通用能力交给框架：鉴权、重试、超时、熔断等由框架统一处理；业务代码只关心“调用什么、结果怎么用”。

## 2. 相关目录与组件

以 Demo 为例，HTTP 调用相关代码通常分布在以下位置：

```
src/Demo/Shared/Remote.Http/
├── Messages/                  # 请求/响应 DTO（跨服务共享）
└── Services/                  # Refit 客户端接口（跨服务共享）
```

## 3. 定义服务调用接口（Refit Client）

### 3.1 接口约束

服务间 HTTP 客户端接口需要继承 `IRestClient`（标记接口，用于限制 `AddRestClient<T>` 的泛型范围）：

- `src/ServiceShared/Remote/Http/IRestClient.cs`

### 3.2 示例：`IAdminRestClient`

以 `src/Demo/Shared/Remote.Http/Services/IAdminRestClient.cs` 为例：

- 用 Refit 特性描述“请求长什么样”：例如 `[Get("/api/admin/dicts/options")]`。
- 用 `[Headers("Authorization: Basic")]` 指定鉴权方式：这里只需要写 Scheme（`Basic`/`Bearer`），Token 由框架自动补齐。

简单理解：

- `Authorization` 的 Scheme 支持 `Basic` 与 `Bearer`。
- `Basic`：更适合“服务内部互调”（不想把用户权限一路传递下去的场景）。
- `Bearer`：更适合“沿用当前用户身份”的场景（会把用户的 Bearer Token 透传给下游）。

## 4. 鉴权与 Token 生成机制

### 4.1 出站请求 Token 注入

`AddRestClient` 默认挂载 `TokenDelegatingHandler`（`src/ServiceShared/Remote/Handlers/TokenDelegatingHandler.cs`）：

- 当 Refit 接口声明了 `Authorization` 头（如 `Basic`/`Bearer`）时，Handler 会根据 Scheme 生成 Token，并写回 `Authorization: {scheme} {token}`。

### 4.2 Basic 与 Bearer 的生成逻辑

- Basic：`src/ServiceShared/Remote/Handlers/Token/BasicTokenGenerator.cs`  
  基于 `BasicOptions`（用户名/密码）生成一个短时效 Token（`BasicTokenValidator.PackToBase64`）。同时会把当前 `UserContext.Id` 打进 Token，用于标识调用方身份。
- Bearer：`src/ServiceShared/Remote/Handlers/Token/BearerTokenGenerator.cs`  
  直接从当前入站请求的 `Authorization` 中截取并透传 Bearer Token（即“带着用户身份去访问下游”）。

### 4.3 Basic 配置

Basic 认证所需配置位于 `appsettings.shared.{Environment}.json`（示例路径见 `docs/wiki/quickstart-zh.md`），典型配置如下：

```json
"Basic": {
  "UserName": "adnc",
  "Password": "your-strong-secret"
}
```

## 5. 注册 HTTP 客户端（AddRestClient）

在应用层依赖注册中，通过 `AddRestClient<T>` 注册 Refit 客户端。以 `src/Demo/Cust/Api/DependencyRegistrar.cs` 为例：

- 先生成默认 Polly 策略：`GenerateDefaultRefitPolicies()`（`src/ServiceShared/Application/Extensions/DependencyRegistrarExtension.cs`）。
- 再注册客户端：`AddRestClient<IAdminRestClient>(ServiceAddressConsts.AdminDemoService, restPolicies)`。

其中 `ServiceAddressConsts`（`src/Demo/Shared/Remote.Http/ServiceAddressConsts.cs`）定义了服务名常量，用于与配置中的 `RpcInfo:Address` 节点匹配。

## 6. 服务发现与地址配置（RegisterType + RpcInfo）

`AddRestClient` 会读取：

- `RegisterType`：决定使用直连地址、CoreDNS 地址或 Consul 地址。
- `RpcInfo`：包含 `Polly` 开关与各服务地址列表。

配置示例（示意）：

```json
"RegisterType": "Direct",
"RpcInfo": {
  "Polly": { "Enable": true },
  "Address": [
    {
      "Service": "adnc-demo-admin-api",
      "Direct": "http://localhost:50010",
      "Consul": "http://adnc-demo-admin-api",
      "CoreDns": "http://adnc-demo-admin-api.default.svc.cluster.local"
    }
  ]
}
```

说明：

- `Direct`：本地/开发最常用，直接填 URL。
- `Consul`：服务注册发现模式。地址配置为服务名，并会挂载 `ConsulDiscoverDelegatingHandler` 做实例发现与路由。
- `CoreDns`：K8S 场景常用，通过集群内部域名访问。

## 7. 调用示例（Controller / AppService）

以 `src/Demo/Cust/Api/Controllers/RestClientDemoController.cs` 为例，Controller 通过构造函数注入 `IAdminRestClient` 并发起调用：

- `GetDictOptionsAsync`：调用管理服务获取字典选项列表。
- `GetSysConfigListAsync`：调用管理服务获取系统配置列表。

建议：

- API 层可以发起调用，但更推荐把跨服务调用放在服务层（`Application`），便于统一管理异常处理与重试规则。
- 下游服务的异常（超时、5xx、熔断）建议转换为统一的业务错误输出，避免把下游细节直接暴露给前端。

## 8. 容错策略（Polly）

默认策略由 `GenerateDefaultRefitPolicies()` 提供，包含：

- 重试：遇到超时或 5xx 时进行重试（带等待间隔）。
- 超时：为每次调用设置最大耗时。
- 熔断：连续失败达到阈值后暂停一段时间，避免故障扩散。

如需关闭策略，可通过 `RpcInfo:Polly:Enable` 关闭（不建议在生产环境关闭）。

## 9. 常见问题

- 401/鉴权失败：确认 Refit 接口是否声明了正确的 `Authorization` Scheme（`Basic`/`Bearer`）；确认 `Basic` 配置是否一致；若使用 Bearer 透传，确认入站请求确实携带了 Bearer Token。
- 找不到服务地址：确认 `ServiceAddressConsts.*` 与 `RpcInfo:Address[].Service` 完全一致；确认当前 `RegisterType` 对应的地址字段已配置。
- 调用链太长：同步调用链越长越不稳定，建议用事件驱动拆解，或引入聚合/查询服务减少链路深度。
