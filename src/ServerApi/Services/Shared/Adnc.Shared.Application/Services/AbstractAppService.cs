namespace Adnc.Shared.Application.Services;

public abstract class AbstractAppService : IAppService
{
    public IObjectMapper Mapper => HttpContextUtility.GetCurrentHttpContext().RequestServices.GetRequiredService<IObjectMapper>();

    protected AppSrvResult AppSrvResult() => new();

    protected AppSrvResult<TValue> AppSrvResult<TValue>([NotNull] TValue value) => new(value);

    protected ProblemDetails Problem(HttpStatusCode? statusCode = null, string detail = null, string title = null, string instance = null, string type = null) => new(statusCode, detail, title, instance, type);

    protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;
}