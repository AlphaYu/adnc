namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// Operation log interceptor
/// </summary>
public class OperateLogInterceptor(OperateLogAsyncInterceptor opsLogAsyncInterceptor) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        opsLogAsyncInterceptor.ToInterceptor().Intercept(invocation);
    }
}
