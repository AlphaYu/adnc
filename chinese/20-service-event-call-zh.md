# ADNC 服务之间如何通过事件（CAP）通信

[GitHub 仓库地址](https://github.com/alphayu/adnc)

在微服务中，除了“同步调用”（HTTP/gRPC）之外，更推荐使用“事件驱动”来做跨服务协作：一个服务只负责把“发生了什么”发布出去，其他服务按需订阅并处理。这样可以降低耦合、减少调用链长度，并更容易实现最终一致性。

本文以 `src/Demo/Cust/Api/Application/Subscribers/CustomerRechargedEventSubscriber.cs` 为例，介绍 ADNC 中基于 CAP（DotNetCore.CAP）的事件发布与订阅方式，并与 `docs/wiki/service-http-call-zh.md` 的“同步调用”做出区分。

---

## 0. 什么时候用事件，什么时候用 HTTP/gRPC？

- 适合用事件：跨服务写操作协作、最终一致性流程（例如支付成功后通知库存、通知积分、通知营销等）。
- 适合用 HTTP/gRPC：需要立即拿到结果的查询/校验（例如取字典、取配置、校验某个状态）。

简单判断：**如果调用方必须等待下游返回结果才能继续当前请求流程**，用 HTTP/gRPC；**如果只是“告诉别人发生了什么”，不要求立刻返回结果**，用事件。

## 1. 快速上手（4 步）

1. 定义事件 DTO（共享）：放在 `src/Demo/Shared/Remote.Event/`，例如 `CustomerRechargedEvent`。
2. 发布事件：在业务服务中注入 `IEventPublisher` 并调用 `PublishAsync(...)`。
3. 订阅事件：写一个 `ICapSubscribe` 订阅者类，使用 `[CapSubscribe("TopicName")]` 标记处理方法（TopicName 可以理解为“事件名称”）。
4. 接入 CAP：在依赖注册里调用 `AddCapEventBus([...])`，把订阅者注册进去。

## 2. 事件 DTO 如何定义（共享契约）

示例：`src/Demo/Shared/Remote.Event/CustomerRechargedEvent.cs`

- 事件 DTO 继承 `BaseEvent`（`src/Infrastructures/EventBus/BaseEvent.cs`），具备统一字段：
  - `Id`：事件唯一标识（强烈建议全局唯一，用于去重）
  - `OccurredDate`：事件发生时间
  - `EventSource`：触发事件的方法/来源（便于排查）
- 事件正文只保留“下游真正需要的数据”，避免塞入过多上下文。

> 约定：默认情况下，事件 Topic（主题）名使用 **类型名**（`typeof(T).Name`）。例如发布 `CustomerRechargedEvent`，Topic 名就是 `"CustomerRechargedEvent"`（见 `src/Infrastructures/EventBus/Cap/CapPublisher.cs`）。

## 3. 如何发布事件（Publish）

示例：`src/Demo/Cust/Api/Application/Services/CustomerService.cs` 的 `RechargeAsync`

核心流程是：

1. 先落库一条业务记录（例：`TransactionLog`，状态为 `Processing`）。
2. 再发布事件（例：`CustomerRechargedEvent`），携带必要的主键与金额等字段。

这样做的好处是：即使下游处理失败或延迟，系统仍能通过业务记录追踪处理状态与补偿逻辑。

发布代码形态（示意）：

```csharp
await eventPublisher.PublishAsync(customerRechargedEvent);
```

## 4. 如何订阅事件（Subscribe）

示例：`src/Demo/Cust/Api/Application/Subscribers/CustomerRechargedEventSubscriber.cs`

订阅者类实现 `ICapSubscribe`，并用 `[CapSubscribe(nameof(CustomerRechargedEvent))]` 指定订阅 Topic。CAP 收到消息后会调用对应方法。

## 5. 订阅端必须考虑的两件事：幂等 + 事务

### 5.1 为什么要幂等？

消息系统天然存在重试与重复投递：例如消费失败、网络抖动、服务重启等，都可能导致同一条事件被再次投递。**订阅者必须做到“重复消费不产生副作用”。**

Demo 的做法是“消息去重记录”：

- `CustomerRechargedEventSubscriber` 使用 `MessageTrackerFactory` 创建 `IMessageTracker`（见 `src/ServiceShared/Application/Services/Trackers/MessageTrackerFactory.cs`）。
- 处理前先判断是否处理过：`HasProcessedAsync(eventId, handlerName)`。
- 成功后标记已处理：`MarkAsProcessedAsync(eventId, handlerName)`。

数据库落地实现见：

- `src/ServiceShared/Application/Services/Trackers/DbMessageTrackerService.cs`
- `src/ServiceShared/Repository/EfCoreEntities/EventTracker.cs`（建议对 `EventId + TrackerName` 建唯一索引，防止并发重复写入）

### 5.2 为什么需要事务？

订阅端通常会做多次写操作（例如更新余额 + 更新流水状态）。这些操作要么全部成功，要么全部回滚，否则会出现数据不一致。

Demo 中使用 `IUnitOfWork` 显式开启事务：

- `BeginTransaction()` → 多次更新 → `CommitAsync()`
- 异常则 `RollbackAsync()` 并抛出，让 CAP 触发重试

## 6. CAP 如何接入（注册与配置）

在调用方/订阅方服务的应用层依赖注册中调用 `AddCapEventBus([...])` 注册订阅者。

示例：`src/Demo/Cust/Api/DependencyRegistrar.cs` 中：

- `AddCapEventBus([typeof(CustomerRechargedEventSubscriber), ...])`

CAP 的公共注册逻辑位于：

- `src/ServiceShared/Application/Registrar/AbstractApplicationDependencyRegistrar.EventBus.cs`

关键点：

- 使用 RabbitMQ 作为消息中间件（从配置的 `RabbitMq` 节点读取）
- 使用 MySQL 作为 CAP 的持久化存储（表前缀 `cap`）
- 默认消费线程数 `ConsumerThreadCount = 1`（保证同一组内的消费顺序更稳定；调大后吞吐更高但顺序不保证）
- 失败会重试（默认最大重试次数、间隔等在注册处设置）

## 7. 一个完整的业务例子：充值事件

以 Demo 的充值为例，可以这样理解整体链路：

1. 用户发起“充值”请求 → Cust 服务创建 `TransactionLog(Processing)`。
2. Cust 服务发布 `CustomerRechargedEvent`（携带 `CustomerId`、`TransactionLogId`、`Amount`）。
3. Cust 服务内（或其他服务）订阅该事件并执行：
   - 更新余额（`Finance`）
   - 更新流水为 `Finished` 并记录变更前后金额
   - 写入去重记录（防重复消费）
4. 任何一步失败 → 回滚事务并抛异常 → CAP 按策略重试，直到成功或达到重试上限。

## 8. 常见问题

- 事件处理方法执行了两次：检查是否做了幂等（MessageTracker）；检查去重表是否建立唯一索引；确认 handlerName 是否稳定（建议用 `nameof(Method)`）。
- 事件发布成功但订阅端没反应：检查 `AddCapEventBus` 是否注册了订阅者；检查 RabbitMQ 与 MySQL 配置；检查 CAP 的 groupName（可理解为“消费组”）是否按环境区分。
- 事件里该放哪些字段：只放“下游必须的数据”（通常是业务主键 + 关键数值），需要更多信息时由下游再查库，不要把整个对象塞进事件里。
