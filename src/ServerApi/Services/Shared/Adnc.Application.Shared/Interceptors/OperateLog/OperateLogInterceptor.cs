using Castle.DynamicProxy;

namespace Adnc.Application.Shared.Interceptors
{
    /// <summary>
    /// 操作日志拦截器
    /// </summary>
    public class OperateLogInterceptor : IInterceptor
    {
        private readonly OperateLogAsyncInterceptor _opsLogAsyncInterceptor;

        public OperateLogInterceptor(OperateLogAsyncInterceptor opsLogAsyncInterceptor)
        {
            _opsLogAsyncInterceptor = opsLogAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            this._opsLogAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}