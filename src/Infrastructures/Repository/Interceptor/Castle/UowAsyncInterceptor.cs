using System.Reflection;
using Castle.DynamicProxy;

namespace Adnc.Infra.Repository.Interceptor.Castle;

/// <summary>
/// 工作单元拦截器
/// </summary>
public class UowAsyncInterceptor(IUnitOfWork unitOfWork) : IAsyncInterceptor
{
    /// <summary>
    /// 同步拦截器
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        var attribute = GetAttribute(invocation);

        if (attribute == null)
        {
            invocation.Proceed();
        }
        else
        {
            InternalInterceptSynchronous(invocation, attribute);
        }
    }

    /// <summary>
    /// 异步拦截器 无返回值
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptAsynchronous(IInvocation invocation)
    {
        var attribute = GetAttribute(invocation);

        invocation.ReturnValue = (attribute is null)
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

        invocation.ReturnValue = (attribute is null)
                                                    ? InternalInterceptAsynchronousWithoutUow<TResult>(invocation)
                                                    : InternalInterceptAsynchronous<TResult>(invocation, attribute)
                                                    ;
    }

    /// <summary>
    /// 同步拦截器事务处理
    /// </summary>
    /// <param name="invocation"></param>
    /// <param name="attribute"></param>
    private void InternalInterceptSynchronous(IInvocation invocation, UnitOfWorkAttribute attribute)
    {
        try
        {
            unitOfWork.BeginTransaction(distributed: attribute.Distributed);
            invocation.Proceed();
            unitOfWork.Commit();
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
        finally
        {
            unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// 异步拦截器事务处理-无返回值
    /// </summary>
    /// <param name="invocation"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    private async Task InternalInterceptAsynchronous(IInvocation invocation, UnitOfWorkAttribute attribute)
    {
        try
        {
            unitOfWork.BeginTransaction(distributed: attribute.Distributed);

            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;

            await unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            unitOfWork.Dispose();
        }
    }

    /// <summary>
    /// 异步拦截器事务处理-有返回值
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invocation"></param>
    /// <param name="attribute"></param>
    /// <returns></returns>
    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation, UnitOfWorkAttribute attribute)
    {
        TResult result;

        try
        {
            unitOfWork.BeginTransaction(distributed: attribute.Distributed);

            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            result = await task;

            await unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            unitOfWork.Dispose();
        }

        return result;
    }

    /// <summary>
    /// 异步拦截器无事务处理-无返回值
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    private static async Task InternalInterceptAsynchronousWithoutUow(IInvocation invocation)
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
    private static async Task<TResult> InternalInterceptAsynchronousWithoutUow<TResult>(IInvocation invocation)
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
    private static UnitOfWorkAttribute? GetAttribute(IInvocation invocation)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var attribute = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        return attribute;
    }
}
