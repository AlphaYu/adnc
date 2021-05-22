using Castle.DynamicProxy;

namespace Adnc.Core.Shared.Interceptors
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

        #region old code

        /*
        private void InterceptAsync(IInvocation invocation)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                invocation.Proceed();
                var result = invocation.ReturnValue as Task;
                result.ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                        _unitOfWork.Commit();
                    else
                        _unitOfWork.Rollback();
                }).Wait();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        private bool IsAsyncMethod(MethodInfo method)
        {
            return (
            method.ReturnType == typeof(Task)
            || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }
        */

        #endregion old code
    }
}