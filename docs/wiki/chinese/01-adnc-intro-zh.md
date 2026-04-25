# ADNC 项目导览：一套可落地的 .NET 8 微服务/分布式工程实践

[GitHub 仓库地址](https://github.com/alphayu/adnc)

如果你想找一套既能跑起来、又方便继续扩展，还适合拿来学习的 .NET 微服务基础工程，ADNC 会是一个不错的参考。仓库里既有可复用的基础设施（`Adnc.Infra.*` / `Adnc.Shared.*`），也有完整的 Demo 和配套 Wiki 文档，能帮助你从“先看懂”一步步走到“自己动手改”。

本文会尽量用通俗的话，带你快速了解这个仓库里有什么、它主要解决什么问题，以及推荐的阅读和上手顺序。

---

## 一句话先说明白

ADNC 是一个基于 `.NET 8` 的开源分布式/微服务框架，也可以用于单体项目。它主要围绕这些能力展开：

- 服务注册与发现、配置中心
- 服务间调用（HTTP / gRPC）、负载均衡、容错治理（Polly）
- 认证授权、日志、链路追踪、健康检查、指标
- 缓存、消息队列、事件总线、分布式事务、读写分离

它提供了一套可以直接落地的工程结构和基础设施集成（见 `README_ZH.md`）。

---

## 1. 这个仓库里有什么（按“看得到的成果”来分）

### 1.1 代码目录一览

仓库主体都在 `src` 下（详见 `README_ZH.md`）：

- `src/Infrastructures`：基础设施集成，比如 Consul、EventBus、缓存等。
- `src/ServiceShared`：服务通用层，比如 WebApi 启动封装、远程调用封装、通用中间件等。
- `src/Gateways`：网关（Ocelot）。
- `src/Demo`：可运行的示例微服务，用来展示不同的服务组织方式。

### 1.2 Demo 服务的“示范意义”

Demo 不只是“能跑的样例”，更重要的是它在演示：同一套基础设施和规范下，服务可以按不同复杂度来组织代码（详见 `README_ZH.md`）：

- Admin：经典三层 + 合约分离
- Maint：更紧凑的三层
- Cust：单项目最小结构
- Ord / Whse：带领域层的 DDD 结构

这能帮你在真实项目里做取舍：**不是所有服务都必须 DDD，也不是所有服务都适合堆在一个工程里**。

---

## 2. 一次请求在 ADNC 里大概怎么走（用“感知链路”理解架构）

下面是一条常见链路。本地用 Direct 模式也成立，换成 Consul/CoreDns 只是“怎么找到下游地址”不同：

1. 客户端请求先进入网关（`src/Gateways/Ocelot`）。
2. 网关把请求转发到某个服务的 WebApi（`src/Demo/*/Api`）。
3. 服务通过统一中间件处理异常、鉴权、CORS、Swagger、健康检查等通用流程，这部分由 `ServiceShared` 复用。
4. 业务代码在 Application 层负责“编排”：查库、写库、调用缓存、发事件，必要时再同步调用其他服务。
5. 服务间通信通常有三种方式：
   - HTTP（Refit）：见 `docs/wiki/service-http-call-zh.md`
   - gRPC：见 `docs/wiki/service-grpc-call-zh.md`
   - 事件（CAP）：见 `docs/wiki/service-event-call-zh.md`

你可以把 ADNC 理解为：**把每个服务里都会重复写的那堆胶水代码，集中沉淀到 `Infrastructures/ServiceShared`，让业务代码更专注于业务本身。**

---

## 3. ADNC 重点能力速览（从“你会用到什么”出发）

### 3.1 配置中心（ConfigurationType）

用于“加载配置”。支持本地文件和 Consul KV（详见 `docs/wiki/config-center-zh.md`）：

- `File`：加载运行目录下的 shared appsettings
- `Consul`：从 Consul KV 加载，默认会轮询刷新，并替换 `$SERVICENAME/$SHORTNAME/$RELATIVEROOTPATH` 这些占位符

### 3.2 注册中心（RegisterType）

用于“服务注册与发现”。支持 `Direct/Consul/CoreDns`（配置说明见 `docs/wiki/appsettings-zh.md`，使用介绍见 `docs/wiki/registry-center-zh.md`）：

- `Direct`：开发时最直观，直接写死 URL
- `Consul`：服务启动时注册，调用时按服务名发现实例
- `CoreDns`：K8S 场景下使用集群内域名

### 3.3 服务间通信（HTTP / gRPC / 事件）

建议的使用原则很简单，也最实用：

- **查询/校验**：用 HTTP 或 gRPC，同步拿结果，链路尽量短  
  - HTTP：`docs/wiki/service-http-call-zh.md`
  - gRPC：`docs/wiki/service-grpc-call-zh.md`
- **跨服务写协作/最终一致性**：优先用事件，便于解耦，也能减少调用链  
  - CAP：`docs/wiki/service-event-call-zh.md`

### 3.4 认证授权

ADNC 在“服务互调”场景下提供了更工程化的做法：既支持 Basic，也支持 Bearer 透传，减少业务代码对 Token 处理的重复劳动（见 `docs/wiki/claims-based-authentication-zh.md`）。

### 3.5 缓存、分布式锁、布隆过滤器

缓存相关能力（包括防雪崩、击穿、穿透这些常见问题的处理方式）集中在一篇文档里，适合直接照着落地（见 `docs/wiki/cache-redis-distributedlock-bloomfilter-zh.md`）。

### 3.6 数据访问与事务（EF Core + UnitOfWork）

仓储、工作单元、本地事务/分布式事务、原生 SQL 等，都有对应的 Demo 和文档（可以先从 `docs/wiki/efcore-*.md` 系列开始）。

### 3.7 可观测性与运维友好（日志 / 链路 / 健康检查）

在微服务里，“能看见、好排查”往往比“能写代码”更重要。ADNC 已经把很多基础能力做成统一接入点：

- 链路追踪：SkyAPM（SkyWalking .NET Agent），开启方式见 `docs/wiki/skyapm-tracing-zh.md`。
- 健康检查：服务默认会暴露健康检查端点，也可以配合 Consul 做健康探测（注册中心部分也会用到）。
- 日志与审计：项目有统一的日志配置和输出方式（可以从 `docs/wiki/appsettings-zh.md` 的 Logging 节点开始理解）。

---

## 4. 本地如何跑起来（最少需要改哪些东西）

如果你只想先“跑起来看看”，按 `docs/wiki/quickstart-zh.md` 的顺序就可以。核心就三件事：

1. 通用配置：`src/Demo/Shared/resources/appsettings.shared.Development.json`（Redis、RabbitMQ 等）。
2. 各服务私有配置：`src/Demo/*/Api/appsettings.Development.json`（数据库连接串、端口等）。
3. 数据库脚本：`doc/dbsql/adnc.sql`（一次性导入）。

启动项目时，建议先从这 4 个开始：

- `Adnc.Gateway.Ocelot`
- `Adnc.Demo.Admin.Api`
- `Adnc.Demo.Maint.Api`
- `Adnc.Demo.Cust.Api`

---

## 5. 推荐的学习/接入路径（不绕路）

如果你希望“看懂之后能改，改了之后还能稳”，建议按这个顺序读：

1. `docs/wiki/quickstart-zh.md`：先跑起来
2. `docs/wiki/appsettings-zh.md`：弄清楚关键配置节点
3. `docs/wiki/config-center-zh.md`：配置中心（如果你有多环境/统一下发的需求）
4. `docs/wiki/registry-center-zh.md`：注册中心（如果你要做真实的服务发现和负载均衡）
5. `docs/wiki/service-http-call-zh.md` / `docs/wiki/service-grpc-call-zh.md`：同步调用怎么写、鉴权怎么带、地址怎么配
6. `docs/wiki/service-event-call-zh.md`：事件驱动怎么做幂等、怎么处理事务与重试
7. `docs/wiki/feature-dev-guide-zh.md` + `docs/wiki/api-dev-guide-zh.md` + `docs/wiki/service-dev-guide.md`：跟着规范新增一个 CRUD
8. 需要排查性能或调用链时读 `docs/wiki/skyapm-tracing-zh.md`
9. 需要部署到服务器或容器时读 `docs/wiki/quickly-docker-deploy-zh.md`

---

## 6. 结语：把它当“框架”，也把它当“样板工程”

ADNC 的价值不只在于“封装了多少功能”，更在于它把一套微服务工程里常见的问题，用**可运行的 Demo + 可阅读的文档 + 可复用的基础设施代码**串了起来。

你可以：

- 把它当成“从零搭微服务底座”的起点；
- 也可以只取其中一部分（比如远程调用封装、配置/注册中心接入、事件驱动模板）接入到你现有的系统里。
