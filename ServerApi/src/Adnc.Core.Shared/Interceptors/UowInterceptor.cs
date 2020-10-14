using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Adnc.Core.Shared.Interceptors
{
    public class UowInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;

        public UowInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;

            //Sync
            if (!IsAsyncMethod(methodInfo))
            {
                try
                {
                    _unitOfWork.BeginTransaction();
                    invocation.Proceed();
                    _unitOfWork.Commit();
                }
                catch(Exception ex)
                {
                    _unitOfWork.Rollback();
                    throw ex;
                }
                finally
                {
                    _unitOfWork.Dispose();
                }
                return;
            }

            //Async
            //InterceptAsync(invocation).Wait();
            InterceptAsync(invocation);
        }

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

        //private async Task InterceptAsync(IInvocation invocation)
        //{
        //    using var transAsync = await _unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        invocation.Proceed();
        //        var result = invocation.ReturnValue as Task;
        //        await result.ContinueWith(async x =>
        //        {
        //            if (x.Status == TaskStatus.RanToCompletion)
        //                await transAsync.CommitAsync();
        //            else
        //                await transAsync.RollbackAsync();
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await transAsync.RollbackAsync();
        //        throw ex;
        //    }
        //}

        private bool IsAsyncMethod(MethodInfo method)
        {
            return (
            method.ReturnType == typeof(Task)
            || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }
    }
}
