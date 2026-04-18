using System.Text.Json;

namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// Operation log interceptor
/// </summary>
public sealed class OperateLogAsyncInterceptor(UserContext userContext, ILogger<OperateLogAsyncInterceptor> logger) : IAsyncInterceptor
{
    /// <summary>
    /// Synchronous interceptor
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        var attribute = GetAttribute(invocation);
        if (attribute == null)
        {
            invocation.Proceed();
        }
        else
        {
            InternalInterceptSynchronous(invocation, attribute);
        }
    }

    /// <summary>
    /// Asynchronous interceptor without a return value
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
    /// Asynchronous interceptor with a return value
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

    private void InternalInterceptSynchronous(IInvocation invocation, OperateLogAttribute attribute)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var fullName = methodInfo.DeclaringType?.FullName ?? string.Empty;
        var startTime = DateTime.Now;
        var log = CreateOpsLog(fullName, methodInfo.Name, attribute.LogName, invocation.Arguments, userContext);
        try
        {
            invocation.Proceed();
            log.Succeed = true;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            WriteOpsLog(log);
        }
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation, OperateLogAttribute attribute)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var fullName = methodInfo.DeclaringType?.FullName ?? string.Empty;
        var startTime = DateTime.Now;
        var log = CreateOpsLog(fullName, methodInfo.Name, attribute.LogName, invocation.Arguments, userContext);

        try
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;
            log.Succeed = true;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            WriteOpsLog(log);
        }
    }

    private static async Task InternalInterceptAsynchronousWithOutOpsLog(IInvocation invocation)
    {
        invocation.Proceed();
        var task = (Task)invocation.ReturnValue;
        await task;
    }

    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation, OperateLogAttribute attribute)
    {
        TResult result;

        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var fullName = methodInfo.DeclaringType?.FullName ?? string.Empty;
        var startTime = DateTime.Now;
        var log = CreateOpsLog(fullName, methodInfo.Name, attribute.LogName, invocation.Arguments, userContext);

        try
        {
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            result = await task;
            log.Succeed = true;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            log.ExecutionTime = (int)(DateTime.Now - startTime).TotalMilliseconds;
            WriteOpsLog(log);
        }
        return result;
    }

    private static async Task<TResult> InternalInterceptAsynchronousWithOutOpsLog<TResult>(IInvocation invocation)
    {
        TResult result;
        invocation.Proceed();
        var task = (Task<TResult>)invocation.ReturnValue;
        result = await task;

        return result;
    }

    private static OperationLog CreateOpsLog(string className, string methodName, string logName, object[] arguments, UserContext userContext)
    {
        var message = string.Empty;
        if (arguments is not null)
        {
            message = JsonSerializer.Serialize(arguments, SystemTextJson.GetAdncDefaultOptions());
            if (message.Length > 1000)
            {
                message = message.Substring(0, 1000);
            }
        }

        var log = new OperationLog
        {
            Id = IdGenerater.GetNextId(),
            ClassName = className,
            CreateTime = DateTime.Now,
            LogName = logName,
            LogType = "Operation Log",
            Message = message,
            Method = methodName,
            Succeed = false,
            UserId = userContext.Id,
            Name = userContext.Name,
            Account = userContext.Account,
            RemoteIpAddress = userContext.RemoteIpAddress
        };
        return log;
    }

    private void WriteOpsLog(OperationLog logInfo)
    {
        try
        {
            //var properties = _mqProducer.CreateBasicProperties();
            //// Enable message persistence
            //properties.Persistent = true;
            //_mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.OpsLog, logInfo, properties);
            Channels.ChannelAccessor<OperationLog>.Instance.Writer.WriteAsync(logInfo).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "WriteOpsLog error");
        }
    }

    /// <summary>
    /// Gets the interceptor attribute
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    private static OperateLogAttribute? GetAttribute(IInvocation invocation)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var attribute = methodInfo.GetCustomAttribute<OperateLogAttribute>();
        return attribute;
    }
}
