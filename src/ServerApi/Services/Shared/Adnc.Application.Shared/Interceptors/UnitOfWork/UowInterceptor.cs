using Castle.DynamicProxy;

namespace Adnc.Application.Shared.Interceptors
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UowInterceptor : IInterceptor
    {
        private readonly UowAsyncInterceptor _uowAsyncInterceptor;

        public UowInterceptor(UowAsyncInterceptor uowAsyncInterceptor)
        {
            _uowAsyncInterceptor = uowAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            this._uowAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}