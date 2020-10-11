using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Adnc.Maint.Core.Entities;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Common.Consts;
using Adnc.Core.Maint.Entities;
using System.Collections.Generic;

namespace Adnc.Maint.Application.Mq
{
    public sealed class OpsLogMqConsumer : BaseRabbitMqConsumer
    {
        // 因为Process函数是委托回调,直接将其他Service注入的话两者不在一个scope,
        // 这里要调用其他的Service实例只能用IServiceProvider CreateScope后获取实例对象
        private readonly IServiceProvider _services;
        private readonly ILogger<OpsLogMqConsumer> _logger;
        private static string s_QueryName = "q-adnc-maint-opslog";

        public OpsLogMqConsumer(IOptions<RabbitMqConfig> options
           , IHostApplicationLifetime appLifetime
           , ILogger<OpsLogMqConsumer> logger
           , IServiceProvider services) 
            : base(options
                  , appLifetime
                  , logger, ExchangeType.Direct
                  , MqConsts.Exchanges.Logs
                  , new[] { MqConsts.RoutingKeys.OpsLog }
                  , s_QueryName
                  , MqConsts.Exchanges.Dead
                  , new Dictionary<string, object>()
                  {
                     //设置当前队列的DLX
                    { "x-dead-letter-exchange",MqConsts.Exchanges.Dead} 
                    //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
                    ,{ "x-dead-letter-routing-key",MqConsts.RoutingKeys.OpsLog}
                    //设置消息的存活时间，即过期时间(毫秒)
                    ,{ "x-message-ttl",1000*60}
                  })
        {
            _services = services;
            _logger = logger;
        }

        protected async override Task<bool> Process(string exchage, string routingKey, string message)
        {
            bool result = false;
            try
            {
                using (var scope = _services.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IMongoRepository<SysOperationLog>>();
                    var entity = JsonSerializer.Deserialize<SysOperationLog>(message);
                    await repository.AddAsync(entity);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return result;
        }
    }
}
