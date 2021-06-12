using Castle.DynamicProxy;

namespace Adnc.Application.Shared.Interceptors
{
    /// <summary>
    /// 操作日志拦截器
    /// </summary>
    public class OpsLogInterceptor : IInterceptor
    {
        private readonly OpsLogAsyncInterceptor _opsLogAsyncInterceptor;

        public OpsLogInterceptor(OpsLogAsyncInterceptor opsLogAsyncInterceptor)
        {
            _opsLogAsyncInterceptor = opsLogAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            this._opsLogAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}