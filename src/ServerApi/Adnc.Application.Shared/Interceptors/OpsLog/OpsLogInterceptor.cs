using System;
using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;
using Adnc.Common.Models;
using Adnc.Common.MqModels;
using Adnc.Common.Consts;
using Adnc.Infr.Mq.RabbitMq;

namespace Adnc.Application.Shared.Interceptors
{
    public class OpsLogInterceptor : IInterceptor
    {
        private bool _isLoging = false;
        private readonly UserContext _userContext;
        private readonly RabbitMqProducer _mqProducer;

        public OpsLogInterceptor(UserContext userContext
            , RabbitMqProducer mqProducer)
        {
            _userContext = userContext;
            _mqProducer = mqProducer;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            var serviceMethod = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = serviceMethod.GetCustomAttribute<OpsLogAttribute>();
            if (attribute == null)
                return;

            if (_isLoging)
                return;
            else
                _isLoging = true;

            var log = new OpsLogMqModel
            {
                ClassName = serviceMethod.DeclaringType.FullName,
                CreateTime = DateTime.Now,
                LogName = attribute.LogName,
                LogType = "操作日志",
                Message = JsonSerializer.Serialize(invocation.Arguments),
                Method = serviceMethod.Name,
                Succeed = "",
                UserId = _userContext.ID,
                UserName = _userContext.Name,
                Account = _userContext.Account,
                RemoteIpAddress = _userContext.RemoteIpAddress
            };

            
            var properties = _mqProducer.CreateBasicProperties();
            //设置消息持久化
            properties.Persistent = true;
            _mqProducer.BasicPublish(MqConsts.Exchanges.Logs, MqConsts.RoutingKeys.OpsLog, log, properties);

        }
    }
}
