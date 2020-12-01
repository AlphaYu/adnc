using System.Reflection;
using Castle.DynamicProxy;

namespace Adnc.Application.Shared.Interceptors
{
    /// <summary>
    /// 操作日志拦截器
    /// </summary>
    public class OpsLogInterceptor : IInterceptor
    {
        private bool _isLoging = false;
        private readonly OpsLogAsyncInterceptor _opsLogAsyncInterceptor;

        public OpsLogInterceptor(OpsLogAsyncInterceptor opsLogAsyncInterceptor)
        {
            _opsLogAsyncInterceptor = opsLogAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = methodInfo.GetCustomAttribute<OpsLogAttribute>();
            if (attribute == null || _isLoging)
            {
                invocation.Proceed();
                return;
            }

            _isLoging = true;

            this._opsLogAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
