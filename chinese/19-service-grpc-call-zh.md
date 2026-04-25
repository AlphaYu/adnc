# ADNC 服务之间如何通过 gRPC 调用

[GitHub 仓库地址](https://github.com/alphayu/adnc)

在 ADNC 中，服务间同步调用除了 HTTP（Refit）外，也常用 gRPC。gRPC 的优点是性能更好、契约更强（基于 `.proto`），适合服务之间的内部调用。本文以 `src/Demo/Cust/Api/Controllers/GrpcClientDemoController.cs` 为例，说明 Cust 服务如何通过 gRPC 调用 Admin 服务。

---

## 0. 快速上手（4 步）

1. 定义 `.proto`：放在共享目录，例如 `src/Demo/Shared/protos/services/admingrpc.proto`。
2. 生成客户端代码：调用方引用 `src/Demo/Shared/Remote.Grpc/Adnc.Demo.Remote.Grpc.csproj`（该项目会按 `.proto` 生成 gRPC Client）。
3. 注册客户端：在调用方依赖注册里调用 `AddGrpcClient<...>(...)`（例如 `src/Demo/Cust/Api/DependencyRegistrar.cs`）。
4. 实际调用：注入生成的 `*GrpcClient`，构造 Request + Metadata（例如 `GrpcClientConsts.BasicHeader`），发起调用。

## 1. 设计建议（写给使用者）

- 尽量用在“查询/校验”这类读操作：跨服务同步调用链越长，越容易放大延迟与故障面。涉及一致性时优先考虑事件驱动（如 CAP）。
- 接口与 DTO 统一从 `.proto` 生成：不要跨服务共享实体模型，也不要在业务代码里手写“对齐字段”的临时结构。
- gRPC 也要治理：超时、重试、熔断等应保持统一策略，避免某个调用点“无限等待”拖垮线程池。

## 2. 相关目录与组件

以 Demo 为例，gRPC 相关代码通常分布在以下位置：

```
src/Demo/Shared/protos/
├── messages/                  # proto 消息
└── services/                  # proto 服务定义

src/Demo/Shared/Remote.Grpc/
└── Adnc.Demo.Remote.Grpc.csproj # 生成客户端代码（Client）
```

服务端（提供 gRPC 的服务）会在自己的 API 工程中：

- 引用 `Grpc.AspNetCore`
- 通过 `<Protobuf ... GrpcServices="Server" />` 生成 Server Base
- 实现 `*GrpcBase` 并映射到 Endpoint

## 3. 定义 gRPC 服务（.proto）

以 `src/Demo/Shared/protos/services/admingrpc.proto` 为例：

- `service AdminGrpc` 定义服务
- `rpc GetSysConfigList(...) returns (...)` 定义方法

说明：

- `.proto` 建议放在共享目录（`src/Demo/Shared/protos`），便于客户端与服务端统一生成。
- 方法入参/出参尽量使用明确的 message 类型，不要过度复用“万能 DTO”。

## 4. 生成客户端与服务端代码

### 4.1 客户端（调用方）

调用方通常只需要引用 `src/Demo/Shared/Remote.Grpc/Adnc.Demo.Remote.Grpc.csproj`。

该项目在 `Adnc.Demo.Remote.Grpc.csproj` 中配置了：

- `<Protobuf Include="..\\protos\\messages\\*.proto" GrpcServices="Client" ... />`
- `<Protobuf Include="..\\protos\\services\\*.proto" GrpcServices="Client" ... />`

因此只要新增 `.proto` 文件，客户端代码会自动生成（按项目配置的通配符生效）。

### 4.2 服务端（被调用方）

以 Admin 服务为例：

- gRPC 实现类：`src/Demo/Admin/Api/Grpc/AdminGrpcServer.cs`
  - 继承 `AdminGrpc.AdminGrpcBase`
  - 实现 proto 中声明的 RPC 方法
- 注册 gRPC：`src/Demo/Admin/Api/DependencyRegistrar.cs` 调用 `_services.AddGrpc();`
- 映射 Endpoint：`src/Demo/Admin/Api/MiddlewareRegistrar.cs` 中 `endpoint.MapGrpcService<AdminGrpcServer>();`

## 5. 端口与 Kestrel 配置（HTTP/2）

gRPC 需要 HTTP/2。Demo 中通常单独开一个 gRPC 端口：

- Admin：`src/Demo/Admin/Api/appsettings.Development.json` 中 `50011`
- Cust：`src/Demo/Cust/Api/appsettings.Development.json` 中 `50031`

配置示例（截取）：

```json
"Kestrel": {
  "Endpoints": {
    "Default": { "Url": "http://0.0.0.0:50010" },
    "Grpc": { "Url": "http://0.0.0.0:50011", "Protocols": "Http2" }
  }
}
```

## 6. 注册 gRPC 客户端（AddGrpcClient）

在调用方（例如 Cust）应用层依赖注册中：

- 生成默认策略：`GenerateDefaultGrpcPolicies()`（与 HTTP 默认策略一致）
- 注册客户端：`AddGrpcClient<AdminGrpc.AdminGrpcClient>(ServiceAddressConsts.AdminDemoService, policies)`

`AddGrpcClient` 的实现位于：

- `src/ServiceShared/Application/Registrar/AbstractApplicationDependencyRegistrar.RpcClient.cs`

它会根据 `RegisterType` 与 `RpcInfo:Address` 选择直连/Consul/CoreDNS 地址，并配置负载均衡（RoundRobin）与 Token 处理等。

## 7. 调用时如何带上鉴权信息（Metadata）

gRPC 调用时通常通过 `Metadata` 传递 Header。Demo 做法是“只传 Scheme，让框架补 Token”：

- `GrpcClientConsts.BasicHeader`：只设置 `Authorization: Basic`
- `GrpcClientConsts.BearerHeader`：只设置 `Authorization: Bearer`

对应代码：

- `src/Demo/Shared/Remote.Grpc/GrpcClientConsts.cs`

框架会通过 `TokenDelegatingHandler` 生成并补齐 Token（最终会变成 `Authorization: Basic {token}` 或 `Authorization: Bearer {token}`）。

## 8. 调用示例（Demo）

示例 Controller：`src/Demo/Cust/Api/Controllers/GrpcClientDemoController.cs`

- 获取字典选项：
  - 构造 `DictOptionRequest`
  - 调用 `adminGrpcClient.GetDictOptionsAsync(request, GrpcClientConsts.BasicHeader)`
- 获取系统配置：
  - 构造 `SysConfigSimpleRequest`
  - 调用 `adminGrpcClient.GetSysConfigListAsync(request, GrpcClientConsts.BasicHeader)`

建议：

- API 层可以发起调用，但更推荐把 gRPC 调用放在服务层（`Application`），便于统一管理异常处理与重试规则。
- 下游超时/熔断等异常建议转换为统一的业务错误输出，避免直接暴露给前端。

## 9. 容错策略（Polly）

默认策略来自 `GenerateDefaultRefitPolicies()`（gRPC 复用同一套策略），包括：

- 重试：遇到超时或 5xx 时重试（带等待间隔）。
- 超时：为每次调用设置最大耗时。
- 熔断：连续失败达到阈值后暂停一段时间，避免故障扩散。

## 10. 常见问题

- gRPC 调不通：优先检查被调用方是否已开启 gRPC 端口且 `Protocols` 为 `Http2`；再检查调用方地址是否指向 gRPC 端口（Direct 模式下 Demo 约定为 “HTTP 端口 + 1”）。
- 401/鉴权失败：确认调用时传入了 `GrpcClientConsts.BasicHeader` 或 `GrpcClientConsts.BearerHeader`；确认 `Basic` 配置一致；若使用 Bearer，确认入站请求携带 Bearer Token。
- Consul 模式找不到服务：确认 `RpcInfo:Address[].Service` 与 `ServiceAddressConsts.*` 一致，且服务已注册。

