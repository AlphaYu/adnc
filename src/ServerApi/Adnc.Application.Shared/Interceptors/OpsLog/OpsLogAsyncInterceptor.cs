using System;
using System.Dynamic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Adnc.Infr.Common;
using Adnc.Infr.Common.Helper;
using Adnc.Infr.Mq.RabbitMq;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace Adnc.Application.Shared.Interceptors
{
    /// <summary>
    /// 操作日志拦截器
    /// </summary>
    public sealed class OpsLogAsyncInterceptor : IAsyncInterceptor
    {
        private readonly UserContext _userContext;
        private readonly RabbitMqProducer _mqProducer;
        private readonly ILogger<OpsLogAsyncInterceptor> _logger;

        public OpsLogAsyncInterceptor(UserContext userContext
            , RabbitMqProducer mqProducer
            , ILogger<OpsLogAsyncInterceptor> logger)
        {
            _userContext = userContext;
            _mqProducer = mqProducer;
            _logger = logger;
        }

        /// <summary>
        /// 同步拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);
            if (attribute == null)
                invocation.Proceed();
            else
                InternalInterceptSynchronous(invocation, attribute);
        }

        /// <summary>
        /// 异步拦截器 无返回值
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
                                     ? InternalInterceptAsynchronousWithOutOpsLog(invocation)
                                     : InternalInterceptAsynchronous(invocation, attribute)
                                     ;
        }

        /// <summary>
        /// 异步拦截器 有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
                                     ? InternalInterceptAsynchronousWithOutOpsLog<TResult>(invocation)
                                     : InternalInterceptAsynchronous<TResult>(invocation, attribute)
                                     ;
        }

        private void InternalInterceptSynchronous(IInvocation invocation,OpsLogAttribute attribute)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var log = CreateOpsLog(methodInfo.DeclaringType.FullName, methodInfo.Name, attribute.LogName, invocation.Arguments, _userContext);
            try
            {
                invocation.Proceed();
                log.Succeed = "true";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                WriteOpsLog(log);
            }
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation,OpsLogAttribute attribute)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var log = CreateOpsLog(methodInfo.DeclaringType.FullName, methodInfo.Name, attribute.LogName, invocation.Arguments, _userContext);

            try
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                log.Succeed = "true";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                WriteOpsLog(log);
            }
        }

        private async Task InternalInterceptAsynchronousWithOutOpsLog(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;

        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation,OpsLogAttribute attribute)
        {
            TResult result;

            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var log = CreateOpsLog(methodInfo.DeclaringType.FullName, methodInfo.Name, attribute.LogName, invocation.Arguments, _userContext);

            try
            {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
                log.Succeed = "true";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                WriteOpsLog(log);
            }
            return result;
        }

        private async Task<TResult> InternalInterceptAsynchronousWithOutOpsLog<TResult>(IInvocation invocation)
        {
            TResult result;
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            result = await task;

            return result;
        }


        private dynamic CreateOpsLog(string className, string methodName, string logName, object[] arguments, UserContext userContext)
        {
            dynamic log = new ExpandoObject();
            log.ClassName = className;
            log.CreateTime = DateTime.Now;
            log.LogName = logName;
            log.LogType = "操作日志";
            log.Message = JsonSerializer.Serialize(arguments, SystemTextJsonHelper.GetAdncDefaultOptions());
            log.Method = methodName;
            log.Succeed = "false";
            log.UserId = userContext.Id;
            log.UserName = userContext.Name;
            log.Account = userContext.Account;
            log.RemoteIpAddress = userContext.RemoteIpAddress;
            return log;
        }

        private void WriteOpsLog(dynamic logInfo)
        {
            try
            {
                var properties = _mqProducer.CreateBasicProperties();
                //设置消息持久化
                properties.Persistent = true;
                _mqProducer.BasicPublish(BaseMqExchanges.Logs, BaseMqRoutingKeys.OpsLog, logInfo, properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取拦截器attrbute
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private OpsLogAttribute GetAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = methodInfo.GetCustomAttribute<OpsLogAttribute>();
            return attribute;
        }
    }
}
