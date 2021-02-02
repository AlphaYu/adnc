using System;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using DotNetCore.CAP;

namespace Adnc.Core.Shared.Interceptors
{
    /// <summary>
    /// 工作单元异步拦截器
    /// </summary>
    public class UowAsyncInterceptor : IAsyncInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICapPublisher _capPublisher;
        private readonly ICapTransaction _capTransaction;

        public UowAsyncInterceptor(IUnitOfWork unitOfWork
            , ICapPublisher capPublisher = null
            , ICapTransaction capTransaction=null)
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
            var attribute = GetAttribute(invocation);
            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            try
            {
                using (var trans = _unitOfWork.GetDbContextTransaction())
                {
                    if (_capPublisher != null && attribute.SharedToCap)
                    {
                        _capPublisher.Transaction.Value = _capTransaction;
                        _capPublisher.Transaction.Value.Begin(trans, autoCommit: false);
                    }

                    invocation.Proceed();

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task InternalInterceptAsynchronous(IInvocation invocation)
        {
            var attribute = GetAttribute(invocation);
            if (attribute == null)
            {
                invocation.Proceed();
                var task = (Task)invocation.ReturnValue;
                await task;
                return;
            }

            try
            {
                using (var trans = _unitOfWork.GetDbContextTransaction())
                {
                    if (_capPublisher != null && attribute.SharedToCap)
                    {
                        _capPublisher.Transaction.Value = _capTransaction;
                        _capPublisher.Transaction.Value.Begin(trans, autoCommit: false);
                    }

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

        private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
        {
            TResult result;

            var attribute = GetAttribute(invocation);
            if (attribute == null)
            {
                invocation.Proceed();
                var task = (Task<TResult>)invocation.ReturnValue;
                result = await task;
                return result;
            }

            try
            {
                using (var trans = _unitOfWork.GetDbContextTransaction())
                {
                    if (_capPublisher != null && attribute.SharedToCap)
                    {
                        _capPublisher.Transaction.Value = _capTransaction;
                        _capPublisher.Transaction.Value.Begin(trans, autoCommit: false);
                    }

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

        public UnitOfWorkAttribute GetAttribute(IInvocation invocation)
        {
            var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
            var attribute = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
            return attribute;
        }
    }
}
