namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// 工作单元拦截器
/// </summary>
public class UowInterceptor(UowAsyncInterceptor uowAsyncInterceptor) : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        uowAsyncInterceptor.ToInterceptor().Intercept(invocation);
    }
}