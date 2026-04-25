# ADNC 如何开启 SkyAPM（SkyWalking）链路追踪

[GitHub 仓库地址](https://github.com/alphayu/adnc)

本项目已集成 SkyAPM（SkyWalking .NET Agent）相关依赖。开启后，你可以在 SkyWalking UI 中看到请求链路（Trace）、服务依赖关系、接口耗时等信息，并能把 HTTP、gRPC、CAP 消息消费/发布、Redis 缓存等操作串起来排查问题。

---

## 0. 前置条件

- 已部署 SkyWalking OAP（后端）。本项目通过 gRPC 上报数据，因此需要确保 `SkyWalking:Transport:gRPC:Servers` 指向可访问的 OAP 地址（默认示例为 `127.0.0.1:11800`）。
- 需要追踪的服务已启动，并能读取到 `SkyWalking` 配置节（见下文）。

## 1. 快速开启（推荐按这个顺序）

1. 配置 SkyWalking OAP 地址：修改 `SkyWalking:Transport:gRPC:Servers`。
3. 启用 Agent：设置环境变量 `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore` 并重启服务。

## 2. 配置项在哪里？

Demo 服务的通用配置在：

- `src/Demo/Shared/resources/appsettings.shared.Development.json`

其中已包含 `SkyWalking` 节点（节选）：

```json
"SkyWalking": {
  "ServiceName": "$SERVICENAME",
  "Namespace": "adnc",
  "Transport": {
    "gRPC": {
      "Servers": "127.0.0.1:11800"
    }
  }
}
```

说明：

- `Namespace`：用于区分环境/项目（可选）。
- `Sampling`：采样策略；开发阶段通常全量采样，生产建议按需开启采样并配置 IgnorePaths。

## 3. 本地调试如何启用（Visual Studio）

各 Demo 服务的 `launchSettings.json` 已预留开关（默认注释）。以 Cust 为例：

- `src/Demo/Cust/Api/Properties/launchSettings.json`

将以下配置取消注释即可：

```json
"ASPNETCORE_HOSTINGSTARTUPASSEMBLIES": "SkyAPM.Agent.AspNetCore"
```

## 4. 容器/服务器部署如何启用（Docker）

部署脚本中已给出示例（默认注释），例如：

- `src/deploy_demo.sh`

核心是给容器增加两个环境变量：

- `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES=SkyAPM.Agent.AspNetCore`（启用 Agent）
- `SKYWALKING__SERVICENAME=xxx`（覆盖 `SkyWalking:ServiceName`）

## 5. CAP 与 Redis 的链路补充（可选）

本项目对 CAP 与 Redis 缓存做了 SkyAPM 扩展挂载，相关扩展会自动注册：

- CAP：`src/Infrastructures/EventBus/Extensions/ServiceCollectionExtension.cs`
- Redis 缓存：`src/Infrastructures/Redis.Caching/Extensions/ServiceCollectionExtension.cs`

这意味着在 UI 里你通常能看到：

- HTTP/gRPC 调用
- CAP 发布/消费链路（同一业务流程更容易串起来）
- Redis 缓存相关 span（如果调用路径涉及缓存）

## 6. 验证是否生效（排查思路）

- 服务是否真的启用了 Agent：确认运行时环境变量 `ASPNETCORE_HOSTINGSTARTUPASSEMBLIES` 包含 `SkyAPM.Agent.AspNetCore`。
- OAP 地址是否可达：确认 `SkyWalking:Transport:gRPC:Servers` 指向可访问的地址与端口。
- 日志：查看 `SkyWalking:Logging:FilePath` 指定的日志文件（默认 `txtlogs\\skyapm-{Date}.log`）。

