using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Adnc.Core.Shared.Interceptors
{
    /// <summary>
    /// 工作单元异步拦截器
    /// </summary>
    public class UowAsyncInterceptor : IAsyncInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public UowAsyncInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 同步拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            InternalInterceptSynchronous(invocation);
        }

        /// <summary>
        /// 异步拦截器 无返回值
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }

        /// <summary>
        /// 异步拦截器 有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
        }

        private void InternalInterceptSynchronous(IInvocation invocation)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                invocation.Proceed();
                
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            TResult result;
            try
            {
                _unitOfWork.BeginTransaction();

                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                throw new Exception(ex.Message, ex);
            }
            finally
            {
                _unitOfWork.Dispose();
            }
            return result;
        }
    }
}
