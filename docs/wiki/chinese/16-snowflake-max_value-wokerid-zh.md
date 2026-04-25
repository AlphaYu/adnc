## ADNC Id生成器(雪花算法)

[GitHub 仓库地址](https://github.com/alphayu/adnc)
主键生成方式有多种，例如自增 ID、GUID、Redis 的 `INCR`、雪花算法等。各类方案的优劣已有大量资料，本文不再赘述。ADNC 采用基于 [Yitter](https://github.com/yitter/IdGenerator) 的雪花算法生成 ID，以兼顾分布式环境下的唯一性与高性能。

## Yitter 雪花算法介绍
传统雪花算法的 ID 由 1 位符号位、41 位时间戳、10 位工作机器 ID 和 12 位自增序号组成，总计 64 位（long 类型）。其主要缺点是生成的 ID 过长，超出了 JavaScript Number 类型的最大安全整数，导致 JS 无法正确解析。
Yitter 对传统雪花算法进行了优化，支持自定义“工作机器 ID”和“自增序号”的长度，默认均为 6 位。在此配置下，50 年内生成的 ID 均不会超出 JS Number 类型的最大值。同时，[Yitter](https://github.com/yitter/IdGenerator) 的雪花算法还解决了系统时间回拨问题，并提供了详尽的文档与测试用例。

### Yitter 算法特点

✔ 生成整型数字，随时间单调递增（不保证连续），长度更短，50 年内不会超过 JS Number 类型最大值（默认配置）。

✔ 生成速度更快，是传统雪花算法的 2-5 倍，0.1 秒可生成 50 万个（基于 8 代低压 i7）。

✔ 支持时间回拨处理。例如服务器时间回拨 1 秒，算法可自动适应并生成临界时间的唯一 ID。

✔ 支持手动插入新 ID。当业务需在历史时间生成新 ID 时，预留位可实现每秒生成 5000 个。

✔ 不依赖任何外部缓存和数据库（K8s 环境下自动注册 WorkerId 的动态库依赖 Redis）。

✔ 基础功能开箱即用，无需配置文件或数据库连接。


### Yitter 性能数据

(参数：10位自增序列，1000次漂移最大值)
| 连续请求量   | 5K      | 5W     | 50W    |
| ------------ | ------- | ------ | ------ |
| 传统雪花算法 | 0.0045s | 0.053s | 0.556s |
| 雪花漂移算法 | 0.0015s | 0.012s | 0.113s |

? 极致性能：500 万/s ~ 3000 万/s（所有测试数据均基于 8 代低压 i7 计算）。


### Yitter 如何处理时间回拨

1. 当发生系统时间回拨时，算法采用历史时序的预留序号生成新的 ID。
2. 回拨生成的 ID 序号，默认靠前，也可调整为靠后。
3. 允许时间回拨至算法预设基数（参数可调）。

## Yitter 雪花算法的使用
Yitter 算法有 3 个核心参数需要配置：
| 参数              | 描述                                                         |
| ----------------- | ------------------------------------------------------------ |
| WorkerIdBitLength | 机器码位长，决定 WorkerId 的最大值，默认值 6。长度 6 表示支持 64 个实例。 |
| SeqBitLength      | 序列数位长，默认值 6，决定每毫秒基础生成的 ID 个数。规则：WorkerIdBitLength + SeqBitLength 不超过 22。 |
| WorkerId          | 机器 ID。无默认值，必须全局唯一，且由程序设定。 |

WorkerIdBitLength 与 SeqBitLength 这两个参数可直接在代码中配置：<br/>
```csharp
public static class IdGenerater
{
    private static bool _isSet = false;
    private static readonly object _locker = new();

    public static byte WorkerIdBitLength => 6;
    public static byte SeqBitLength => 6;
    public static short MaxWorkerId => (short)(Math.Pow(2.0, WorkerIdBitLength) - 1);
    public static short CurrentWorkerId { get; private set; } = -1;
    
    // 其它业务逻辑
}
```
### 如何获取 WorkerId
对于单体架构系统，可直接从配置文件获取 WorkerId；对于分布式或微服务架构，则需要在项目启动时动态获取 WorkerId。ADNC 会预先生成所有 WorkerId，并保存在 Redis 的 zset 中（value = WorkerId，score = 时间戳）。实例启动时通过 Lua 脚本从 zset 获取 score 最小的 WorkerId，并同步更新 score 为当前时间戳。WorkerId 获取后，由定时服务每分钟刷新当前 WorkerId 的 score。
```csharp
namespace Adnc.Infra.IdGenerater.Yitter
{
    public class WorkerNodeHostedService : BackgroundService
    {
        public WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger
            , WorkerNode workerNode
            , string serviceName)
        {
                _serviceName = serviceName;
                _workerNode = workerNode;
                _logger = logger;
        }

        public async override Task StartAsync(CancellationToken cancellationToken)
        {
            //预先生成好所有workerid并保存到redis
            await _workerNode.InitWorkerNodesAsync(_serviceName);
            //获取workerid
            var workerId = await _workerNode.GetWorkerIdAsync(_serviceName);
            //将获取到的workerid赋值给YitterSnowFlake.CurrentWorkerId
            YitterSnowFlake.CurrentWorkerId = (short)workerId;
            await base.StartAsync(cancellationToken);
        }

        
        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            var subtractionMilliseconds = 0 - (_millisecondsDelay * 1.5);
            var score = DateTime.Now.AddMilliseconds(subtractionMilliseconds).GetTotalMilliseconds();
            //实例停止时，回收当前workerid.
            await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId, score);
        }
        
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_millisecondsDelay, stoppingToken);

                    if (stoppingToken.IsCancellationRequested) break;

                    //定时刷新YitterSnowFlake.CurrentWorkerId的score。
                    await _workerNode.RefreshWorkerIdScoreAsync(_serviceName, YitterSnowFlake.CurrentWorkerId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                    await Task.Delay(_millisecondsDelay / 3, stoppingToken);
                }
            }
        }  
    }
}
```
### 如何调用Yitter
```csharp
using Adnc.Infra.IdGenerater.Yitter;

namespace Adnc.XXX.Application.Services
{
    public class xxxAppService : AbstractAppService, IxxxAppService
    {
        var id = IdGenerater.GetNextId(); // 获取Id
    }
}
```
---
—— 完 ——

如有帮助，欢迎 [star & fork](https://github.com/alphayu/adnc)。
