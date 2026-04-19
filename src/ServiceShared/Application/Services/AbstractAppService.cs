namespace Adnc.Shared.Application.Services;

public abstract class AbstractAppService : IAppService
{
    public static IObjectMapper Mapper
    {
        get => ServiceLocator.GetProvider().GetRequiredService<IObjectMapper>();
    }

    protected static ServiceResult ServiceResult() => new();

    protected static ServiceResult<TValue> ServiceResult<TValue>(TValue value)
        where TValue : notnull
    {
        return new ServiceResult<TValue>(value);
    }

    protected static ProblemDetails Problem(HttpStatusCode? statusCode = null, string? detail = null, string? title = null, string? instance = null, string? type = null) => new(statusCode, detail, title, instance, type);

    protected static Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;
}
