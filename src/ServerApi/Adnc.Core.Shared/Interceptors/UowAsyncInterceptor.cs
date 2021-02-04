using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DotNetCore.CAP;

namespace Adnc.Core.Shared.Interceptors
{
    /// <summary>
    /// 工作单元拦截器
    /// </summary>
    public class UowAsyncInterceptor : IAsyncInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICapPublisher _capPublisher;
        private readonly ICapTransaction _capTransaction;

        public UowAsyncInterceptor(IUnitOfWork unitOfWork
            , ICapPublisher capPublisher = null
            , ICapTransaction capTransaction = null)
        {
            _unitOfWork = unitOfWork;
            _capPublisher = capPublisher;
            _capTransaction = capTransaction;
        }

        /// <summary>
        /// 同步拦截器
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptSynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            if (attribute == null)
                invocation.Proceed();
            else
                InternalInterceptSynchronous(invocation, attribute);
        }

        /// <summary>
        /// 异步拦截器 无返回值
        /// </summary>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
                                    ? InternalInterceptAsynchronousWithoutUow(invocation)
                                    : InternalInterceptAsynchronous(invocation, attribute)
                                    ;
        }

        /// <summary>
        /// 异步拦截器 有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);

            invocation.ReturnValue = attribute == null
                                    ? InternalInterceptAsynchronousWithoutUow<TResult>(invocation)
                                    : InternalInterceptAsynchronous<TResult>(invocation,attribute)
                                    ;
        }

        /// <summary>
        /// 同步拦截器事务处理
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="attribute"></param>
        private void InternalInterceptSynchronous(IInvocation invocation, [NotNull] UnitOfWorkAttribute attribute)
        {
            try
            {
                using (var trans = this.GetDbTransaction(attribute))
                {
                    invocation.Proceed();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 异步拦截器事务处理-无返回值
        /// </summary>
        /// <param name="invocation"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private async Task InternalInterceptAsynchronous(IInvocation invocation, [NotNull] UnitOfWorkAttribute attribute)
        {
            try
            {
                using (var trans = this.GetDbTransaction(attribute))
                {
                    invocation.Proceed();
                    var task = (Task)invocation.ReturnValue;
                    await task;

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 异步拦截器事务处理-有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation, [NotNull] UnitOfWorkAttribute attribute)
        {
            TResult result;

            try
            {
                using (var trans = this.GetDbTransaction(attribute))
                {
                    invocation.Proceed();
                    var task = (Task<TResult>)invocation.ReturnValue;
                    result = await task;

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        /// <summary>
        /// 异步拦截器无事务处理-无返回值
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private async Task InternalInterceptAsynchronousWithoutUow(IInvocation invocation)
        {
            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;
        }

        /// <summary>
        /// 异步拦截器无事务处理-有返回值
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private async Task<TResult> InternalInterceptAsynchronousWithoutUow<TResult>(IInvocation invocation)
        {
            TResult result;
            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            result = await task;
            return result;
        }

        /// <summary>
        /// 获取拦截器attrbute
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private UnitOfWorkAttribute GetAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            return attribute;
        }

        /// <summary>
        /// 获取事务
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private dynamic GetDbTransaction(UnitOfWorkAttribute attribute)
        {
            dynamic trans;
            var adncTrans = _unitOfWork.GetDbContextTransaction();

            if (_capPublisher != null && attribute.SharedToCap)
            {
                _capPublisher.Transaction.Value = _capTransaction;
                //var capTrans = _capPublisher.Transaction.Value.Begin(adncTrans, autoCommit: false);
                var capTrans = _capPublisher.Transaction.Value;
                capTrans.DbTransaction = adncTrans;
                capTrans.AutoCommit = false;

                trans = capTrans;
            }
            else
            {
                trans = adncTrans;
            }

            return trans;
        }
    }
}
