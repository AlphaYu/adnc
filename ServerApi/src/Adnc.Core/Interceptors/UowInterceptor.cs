using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Adnc.Core.Interceptors
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
                using var trans = _unitOfWork.BeginTransaction();
                try
                {
                    invocation.Proceed();
                    trans.Commit();
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
                return;
            }

            //Async
            //InterceptAsync(invocation).Wait();
            InterceptAsync(invocation);
        }

        private void InterceptAsync(IInvocation invocation)
        {
            using var transAsync =  _unitOfWork.BeginTransaction();
            try
            {
                invocation.Proceed();
                var result = invocation.ReturnValue as Task;
                result.ContinueWith(x =>
                {
                    if (x.Status == TaskStatus.RanToCompletion)
                        transAsync.Commit();
                    else
                        transAsync.Rollback();
                }).Wait();
            }
            catch (Exception ex)
            {
                transAsync.Rollback();
                throw ex;
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
