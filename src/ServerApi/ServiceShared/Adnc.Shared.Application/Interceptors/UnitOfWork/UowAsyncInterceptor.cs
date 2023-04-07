namespace Adnc.Shared.Application.Interceptors;

/// <summary>
/// 工作单元拦截器
/// </summary>
public class UowAsyncInterceptor : IAsyncInterceptor
{
    private readonly IUnitOfWork _unitOfWork;

    public UowAsyncInterceptor(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

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
    private void InternalInterceptSynchronous(IInvocation invocation, [NotNull] UnitOfWorkAttribute attribute)
    {
        try
        {
            _unitOfWork.BeginTransaction(distributed: attribute.SharedToCap);
            invocation.Proceed();
            _unitOfWork.Commit();
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
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
            _unitOfWork.BeginTransaction(distributed: attribute.SharedToCap);

            invocation.Proceed();
            var task = (Task)invocation.ReturnValue;
            await task;

            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
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
            _unitOfWork.BeginTransaction(distributed: attribute.SharedToCap);

            invocation.Proceed();
            var task = (Task<TResult>)invocation.ReturnValue;
            result = await task;

            await _unitOfWork.CommitAsync();
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
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
    private UnitOfWorkAttribute? GetAttribute(IInvocation invocation)
    {
        var methodInfo = invocation.Method ?? invocation.MethodInvocationTarget;
        var attribute = methodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        return attribute;
    }
}