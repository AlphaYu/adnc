namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// 操作日志拦截器
/// </summary>
public class OperateLogInterceptor(OperateLogAsyncInterceptor opsLogAsyncInterceptor) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        opsLogAsyncInterceptor.ToInterceptor().Intercept(invocation);
    }
}
