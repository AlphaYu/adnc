﻿using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Adnc.Core.Shared.IRepositories;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Core.Maint.Entities;

namespace Adnc.Maint.Application.Mq
{
    /// <summary>
    /// 操作日志消费者
    /// </summary>
    public sealed class OpsLogMqConsumer : BaseRabbitMqConsumer
    {
        // 因为Process函数是委托回调,直接将其他Service注入的话两者不在一个scope,
        // 这里要调用其他的Service实例只能用IServiceProvider CreateScope后获取实例对象
        private readonly IServiceProvider _services;
        private readonly ILogger<OpsLogMqConsumer> _logger;

        public OpsLogMqConsumer(IOptionsSnapshot<RabbitMqConfig> options
           , IHostApplicationLifetime appLifetime
           , ILogger<OpsLogMqConsumer> logger
           , IServiceProvider services) 
            : base(options, appLifetime, logger)
        {
            _services = services;
            _logger = logger;
        }

        /// <summary>
        /// 配置Exchange
        /// </summary>
        /// <returns></returns>
        protected override ExchageConfig GetExchageConfig()
        {
            return new ExchageConfig()
            {
                Name = MqExchanges.Logs
                ,
                Type = ExchangeType.Direct
                ,
                DeadExchangeName = MqExchanges.Dead
            };
        }

        /// <summary>
        /// 设置路由Key
        /// </summary>
        /// <returns></returns>
        protected override string[] GetRoutingKeys()
        {
            return new[] { MqRoutingKeys.OpsLog };
        }

        /// <summary>
        /// 配置队列
        /// </summary>
        /// <returns></returns>
        protected override QueueConfig GetQueueConfig()
        {
            var config = GetCommonQueueConfig();
            config.Name = "q-adnc-maint-opslog";
            config.AutoAck = false;
            config.PrefetchCount = 5;
            config.Arguments = new Dictionary<string, object>()
                  {
                     //设置当前队列的DLX
                    { "x-dead-letter-exchange",MqExchanges.Dead} 
                    //设置DLX的路由key，DLX会根据该值去找到死信消息存放的队列
                    ,{ "x-dead-letter-routing-key",MqRoutingKeys.OpsLog}
                    //设置消息的存活时间，即过期时间(毫秒)
                    ,{ "x-message-ttl",1000*60}
                  };
            return config;
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="exchage">交换机</param>
        /// <param name="routingKey">路由Key</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
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
