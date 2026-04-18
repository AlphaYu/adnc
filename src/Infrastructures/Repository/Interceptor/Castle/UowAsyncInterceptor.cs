using System.Reflection;
using Castle.DynamicProxy;

namespace Adnc.Infra.Repository.Interceptor.Castle;

/// <summary>
/// Unit of work interceptor
/// </summary>
public class UowAsyncInterceptor(IUnitOfWork unitOfWork) : IAsyncInterceptor
{
    /// <summary>
    /// Synchronous interceptor
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
    /// Async interceptor without return value
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
    /// Async interceptor with return value
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
    /// Synchronous interceptor transaction handling
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
    /// Async interceptor transaction handling — no return value
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
    /// Async interceptor transaction handling — with return value
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
    /// Async interceptor without transaction — no return value
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
    /// Async interceptor without transaction — with return value
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
    /// Gets the interceptor attribute.
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
