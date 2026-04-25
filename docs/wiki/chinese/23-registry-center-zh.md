# ADNC 如何使用注册中心

[GitHub 仓库地址](https://github.com/alphayu/adnc)

本项目支持多种“服务注册与发现”方式，通过 `RegisterType` 控制：

- `Direct`：不进行注册，服务间调用直接使用固定 URL。
- `Consul`：注册到 Consul，服务间调用通过“服务名”发现实例地址。
- `CoreDns`：在 K8S 场景使用 CoreDNS（通过集群内域名访问）。

---

## 0. 快速上手（推荐顺序）

1. 启动 Consul（或使用现成环境）。
2. 将服务的 `RegisterType` 切换为 `Consul`。
3. 补齐 `Consul` 节点配置（至少需要 `ConsulUrl`、健康检查相关配置、服务名/标签等）。
4. 启动服务，进入 Consul UI 确认服务已注册且健康检查通过。
5. 将服务间调用的地址切换为 Consul 形式（`RpcInfo:Address[*]:Consul = http://{service-name}`），验证调用正常。

---

## 1. 注册中心与配置中心的区别

本项目中：

- **配置中心** 用 `ConfigurationType` 控制（`File/Consul/...`），用于“加载 appsettings 配置”。详见 `docs/wiki/config-center-zh.md`。
- **注册中心** 用 `RegisterType` 控制（`Direct/Consul/CoreDns`），用于“服务启动时注册 + 服务间调用时发现”。

两者可以配合使用：

- 常见做法：`ConfigurationType = Consul`（配置来自 Consul KV）+ `RegisterType = Consul`（服务也注册到 Consul）。
- 也可以仅使用注册中心：`ConfigurationType = File` + `RegisterType = Consul`（配置走本地文件，但依旧注册到 Consul）。
- 也可以配置中心Consul，注册中心CoreDns：`ConfigurationType = Consul` + `RegisterType = CoreDns`（配置走Consul，注册到 CoreDns）。

---

## 2. 服务启动时，本项目做了什么（注册流程）

各服务在 `Program.cs` 中会调用：

- `app.UseRegistrationCenter()`（见 `src/Demo/*/Api/Program.cs`）

核心逻辑位于 `src/ServiceShared/WebApi/Extensions/HostExtension.cs`：

- 读取 `RegisterType`：
  - `Direct`：不注册
  - `Consul/CoreDns`：调用 `RegisterToConsul(...)`
- 注册与注销时机：
  - `ApplicationStarted`：向 Consul 注册服务
  - `ApplicationStopping`：从 Consul 注销服务

具体的 Consul 注册实现位于 `src/Infrastructures/Consul/Registrar/RegistrationProvider.cs`，注册信息包含：

- `Name`：`Consul:ServiceName`（一般为 `$SERVICENAME`，启动时会自动替换）
- `Address/Port`：来自 `Kestrel:Endpoints:Default:Url`（若是 `0.0.0.0`，会自动替换为本机的一个 IPv4）
- `Tags`：`Consul:ServerTags`（例如 `urlprefix-/$SHORTNAME`，常用于网关路由/分组）
- `Check`：HTTP 健康检查（由 `Consul:HealthCheckUrl` 等配置决定）

---

## 3. 如何切换到注册中心（Consul）

### 3.1 开启 `RegisterType = Consul`

以本地开发为例（默认 `src/Demo/Shared/resources/appsettings.shared.Development.json` 是 `Direct`），你可以在对应环境配置中设置：

```json
{
  "RegisterType": "Consul"
}
```

> 提示：如果你使用“配置中心（Consul KV）”加载 shared 配置，则建议把 `RegisterType` 放到 shared 的 KV 中统一管理。

### 3.2 配置 `Consul` 节点（注册所需）

注册中心至少需要 `ConsulUrl`，以及服务名/健康检查等信息。一个典型配置如下（节选）：

```json
{
  "Consul": {
    "ConsulUrl": "http://127.0.0.1:8500",
    "ServiceName": "$SERVICENAME",
    "ServerTags": [ "urlprefix-/$SHORTNAME" ],
    "HealthCheckUrl": "$RELATIVEROOTPATH/health-24b01005-a76a-4b3b-8fb1-5e0f2e9564fb",
    "HealthCheckIntervalInSecond": 6,
    "DeregisterCriticalServiceAfter": 20,
    "Timeout": 6
  }
}
```

说明：

- `ConsulUrl`：Consul HTTP API 地址（注册与发现都会用到）。
- `ServiceName/ServerTags/HealthCheckUrl`：支持占位符 `$SERVICENAME`、`$SHORTNAME`、`$RELATIVEROOTPATH`，启动时会自动替换（见 `src/ServiceShared/WebApi/Extensions/WebApplicationBuilderExtension.cs`）。
- `HealthCheckUrl`：项目默认会暴露健康检查端点（见 `src/ServiceShared/WebApi/Registrar/AbstractWebApiMiddlewareRegistrar.cs` 的 `UseHealthChecks(...)`）。

### 3.3 确认 `Kestrel` 默认端口（注册地址来源）

本项目注册到 Consul 的地址来自 `Kestrel:Endpoints:Default:Url`。

例如 Demo 的 Cust 服务（`src/Demo/Cust/Api/appsettings.Development.json`）：

```json
{
  "Kestrel": {
    "Endpoints": {
      "Default": { "Url": "http://0.0.0.0:50030" },
      "Grpc": { "Url": "http://0.0.0.0:50031", "Protocols": "Http2" }
    }
  }
}
```

当 `Url` 绑定为 `0.0.0.0` 时，注册时会自动使用本机 IPv4 进行替换，避免注册到不可访问的地址。

---

## 4. 服务间调用如何通过 Consul 发现实例

本项目的服务间调用地址由 `RpcInfo:Address` 决定，并会根据 `RegisterType` 选择不同的 BaseAddress（见 `src/ServiceShared/Application/Registrar/AbstractApplicationDependencyRegistrar.RpcClient.cs`）。

当 `RegisterType = Consul` 时，推荐在 `RpcInfo:Address[*]:Consul` 里配置为“服务名”形式：

```json
{
  "RpcInfo": {
    "Address": [
      {
        "Service": "adnc-demo-admin-api",
        "Consul": "http://adnc-demo-admin-api"
      }
    ]
  }
}
```

### 4.1 HTTP（REST / Refit）

- 当 BaseAddress 为 `http://{service-name}` 时，请求会经过 `ConsulDiscoverDelegatingHandler`（`src/Infrastructures/Consul/Discover/Handler/ConsulDiscoverDelegatingHandler.cs`）。
- Handler 会根据 `{service-name}` 从 Consul 查询健康实例，选出一个实例地址，并把实际请求改写为 `http://{ip}:{port}/{path}`。

### 4.2 gRPC

- 当 BaseAddress 为 `consul://{service-name}` 时，会启用自定义 Resolver（`src/Infrastructures/Consul/Discover/GrpcResolver/ConsulGrpcResolver.cs`）。
- Resolver 会周期性从 Consul 拉取所有健康实例，并配合 gRPC 的 `RoundRobin` 做负载均衡。

> 注意：本项目约定同一服务的 gRPC 端口 = HTTP 端口 + 1（Resolver 内部对端口做了 `+1` 处理）。

---

## 5. 常见问题

- **服务未出现在 Consul UI**：确认 `RegisterType = Consul`；确认 `Consul:ConsulUrl` 正确且可访问；确认服务启动代码调用了 `UseRegistrationCenter()`。
- **Consul 显示不健康**：检查 `Consul:HealthCheckUrl` 是否与项目实际暴露的健康检查路由一致；确认网关/反向代理未拦截该路径。
- **调用时解析不到实例地址**：确认目标服务已注册且健康；确认 `RpcInfo:Address[*]:Consul` 的 host 与注册的 `ServiceName` 一致（通常就是服务名）。

---

## 6. 快速启动 Consul（仓库内脚本）

仓库内提供了 Consul 的 docker-compose 与初始化脚本（包含 KV 初始化；也可直接作为注册中心使用）：

- `doc/devops-staging/adnc-consul/docker-compose.yml`
- `doc/devops-staging/adnc-consul/consul-init.sh`
- `doc/devops/docker-compose/adnc-consul/docker-compose.yml`
- `doc/devops/docker-compose/adnc-consul/consul-init.sh`
