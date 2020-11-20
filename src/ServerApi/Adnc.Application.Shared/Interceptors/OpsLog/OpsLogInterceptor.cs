using System;
using System.Reflection;
using System.Text.Json;
using Castle.DynamicProxy;
using Adnc.Infr.Mq.RabbitMq;
using Adnc.Infr.Common;
using Adnc.Infr.Common.Helper;

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

            var logInfo = new
            {
                ClassName = serviceMethod.DeclaringType.FullName,
                CreateTime = DateTime.Now,
                LogName = attribute.LogName,
                LogType = "操作日志",
                Message = JsonSerializer.Serialize(invocation.Arguments, SystemTextJsonHelper.GetAdncDefaultOptions()),
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
            _mqProducer.BasicPublish(BaseMqExchanges.Logs, BaseMqRoutingKeys.OpsLog, logInfo, properties);

        }
    }
}
