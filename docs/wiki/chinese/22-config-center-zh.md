# ADNC 如何使用配置中心（Consul）

[GitHub 仓库地址](https://github.com/alphayu/adnc)

本项目支持两种配置加载方式：

- `File`：从本地 `appsettings` 文件读取配置，适合本地开发。
- `Consul`：从 Consul KV 读取配置，适合测试/预发/生产环境统一管理与动态下发。

> 说明：代码中已预留 `Nacos/Etcd` 分支，但当前未实现（见 `src/ServiceShared/WebApi/Extensions/WebApplicationBuilderExtension.cs`）。

---

## 0. 快速上手（推荐顺序）

1. 启动 Consul（或使用现成环境）。
2. 在 Consul KV 写入配置（通常是两份：shared + 服务专属）。
3. 将服务的 `ConfigurationType` 切换为 `Consul`，并配置 `ConsulUrl`、`ConsulKeyPath`。
4. 启动服务，确认配置已生效；修改 KV 后等待几秒观察是否自动生效（本项目默认轮询刷新）。

## 1. 配置加载流程（本项目做了什么）

各服务启动时会调用 `AddConfiguration`（`src/ServiceShared/WebApi/Extensions/WebApplicationBuilderExtension.cs`），核心逻辑是：

- 读取 `ConfigurationType`：
  - `File`：加载 `appsettings.shared.{Environment}.json`（位于运行目录 `AppContext.BaseDirectory`）。
  - `Consul`：读取 `Consul` 节点的 `ConsulUrl` 与 `ConsulKeyPath`，并加载对应 KV。
- 加载完成后，项目会自动替换配置中的占位符：
  - `$SERVICENAME`、`$SHORTNAME`、`$RELATIVEROOTPATH`
- 当配置源发生变化并触发 reload 时，会再次执行占位符替换。

## 2. 如何切换到配置中心（Consul）

以 Demo 的 Cust 服务为例，测试/预发/生产环境的 `appsettings.*.json` 已配置为 `Consul`：

- `src/Demo/Cust/Api/appsettings.Staging.json`

结构如下（节选）：

```json
{
  "ConfigurationType": "Consul",
  "Consul": {
    "ConsulUrl": "http://172.80.0.4:8500",
    "ConsulKeyPath": "adnc/staging/shared/appsettings,adnc/staging/$SHORTNAME/appsettings"
  }
}
```

说明：

- `ConsulUrl`：Consul HTTP API 地址。
- `ConsulKeyPath`：Consul KV 的 key 路径，支持逗号分隔多个 key（会按顺序加载）。
- `$SHORTNAME`：启动时会自动替换为当前服务的 shortName（例如 `cust-api`、`admin-api`）。

## 3. KV Key 如何组织（shared + 服务专属）

本项目推荐按“先加载 shared，再加载服务专属”的方式组织配置：

- shared：`adnc/{env}/shared/appsettings`
- 服务专属：`adnc/{env}/{shortName}/appsettings`

通过 `ConsulKeyPath` 一次性加载两份配置：

```text
adnc/staging/shared/appsettings,adnc/staging/$SHORTNAME/appsettings
```

加载顺序很重要：

- `shared` 放在前面：提供各服务通用配置（Redis、RabbitMQ、RpcInfo、SkyWalking 等）。
- 服务专属放在后面：只覆盖该服务需要差异化的部分（例如数据库连接串、端口等）。

KV 的 value 内容是 **JSON 文本**（与 `appsettings.json` 内容一致）。本项目的 Consul Provider 会把 KV value 当作 JSON 配置文件来解析（`src/Infrastructures/Consul/Configuration/DefaultConsulConfigurationProvider.cs`）。

## 4. 配置变更是否会自动生效？

会。

当使用 Consul 配置源时，加载配置会启用 `reloadOnChanges=true`。Consul Provider 会每 3 秒轮询一次 KV（对比 `LastIndex`），发现变化就触发配置 reload（`OnReload()`），并再次执行占位符替换。

注意：

- 自动 reload 只影响“读取配置”的部分。某些组件若在启动时读取一次配置并缓存，仍可能需要重启才能完全生效（取决于组件实现）。
- 建议把“需要热更新”的配置项设计为可动态读取的 Options/Monitor 模式；敏感变更（连接串、证书等）建议通过灰度与重启生效。

## 5. 使用 devops-staging 一键初始化 Consul KV（Demo）

本仓库提供了 staging 环境的 Consul 部署与 KV 初始化脚本：

- Consul Compose：`doc/devops-staging/adnc-consul/docker-compose.yml`
- KV 初始化脚本：`doc/devops-staging/adnc-consul/consul-init.sh`
- KV 初始数据：`doc/devops-staging/adnc-consul/kv.json`

脚本启动后会执行 `consul kv import ... @/consul/kv.json` 导入初始 KV。

提示：

- `kv.json` 中的 `value` 字段是 base64（Consul 的 import 格式要求），导入后 Consul 实际保存的是解码后的 JSON 内容。
- Compose 中默认把 Consul UI 暴露到宿主机端口 `8590`（`doc/devops-staging/adnc-consul/docker-compose.yml`）。

## 6. 常见问题

- 服务启动报错找不到 Consul 配置：检查 `ConfigurationType` 是否为 `Consul`；检查 `ConsulUrl` 可访问；检查 `ConsulKeyPath` 是否存在对应 key。
- 配置修改后未生效：确认修改的是正确的 key；等待 3～5 秒；查看服务日志是否有异常；确认该配置项是否需要重启才能生效。
- shared 与服务专属冲突：确认加载顺序（服务专属应在后面）；尽量只在服务专属里覆盖必要项，避免重复定义大量节点。

