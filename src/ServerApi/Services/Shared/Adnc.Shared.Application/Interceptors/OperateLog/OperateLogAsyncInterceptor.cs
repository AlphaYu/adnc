namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// 操作日志拦截器
/// </summary>
public sealed class OperateLogAsyncInterceptor : IAsyncInterceptor
{
    private readonly UserContext _userContext;
    private readonly ILogger<OperateLogAsyncInterceptor> _logger;

    public OperateLogAsyncInterceptor(UserContext userContext
        , ILogger<OperateLogAsyncInterceptor> logger)
    {
        _userContext = userContext;
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

    private void InternalInterceptSynchronous(IInvocation invocation, OperateLogAttribute attribute)
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

    private async Task InternalInterceptAsynchronous(IInvocation invocation, OperateLogAttribute attribute)
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

    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation, OperateLogAttribute attribute)
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

    private OperationLog CreateOpsLog(string className, string methodName, string logName, object[] arguments, UserContext userContext)
    {
        var log = new OperationLog
        {
            ClassName = className,
            CreateTime = DateTime.Now,
            LogName = logName,
            LogType = "操作日志",
            Message = JsonSerializer.Serialize(arguments, SystemTextJson.GetAdncDefaultOptions()),
            Method = methodName,
            Succeed = "false",
            UserId = userContext.Id,
            UserName = userContext.Name,
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
            ////设置消息持久化
            //properties.Persistent = true;
            //_mqProducer.BasicPublish(MqExchanges.Logs, MqRoutingKeys.OpsLog, logInfo, properties);
            var operationLogWriter = Channels.ChannelHelper<OperationLog>.Instance.Writer;
            operationLogWriter.WriteAsync(logInfo).GetAwaiter().GetResult();
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
    private OperateLogAttribute GetAttribute(IInvocation invocation)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var attribute = methodInfo.GetCustomAttribute<OperateLogAttribute>();
        return attribute;
    }
}