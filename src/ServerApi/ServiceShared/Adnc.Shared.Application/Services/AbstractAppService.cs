using Adnc.Infra.Core.Guard;

namespace Adnc.Shared.Application.Services;

public abstract class AbstractAppService : IAppService
{
    public IObjectMapper Mapper
    {
        get
        {
            var httpContext = InfraHelper.Accessor.GetCurrentHttpContext();
            if (httpContext is not null)
                return httpContext.RequestServices.GetRequiredService<IObjectMapper>();
            if (ServiceLocator.Provider is not null)
            {
                var mapper = ServiceLocator.Provider.GetService<IObjectMapper>();
                return mapper ?? throw new NullReferenceException("'mapper = ServiceLocator.Provider.GetService' is null");
            }
            throw new NotImplementedException();
        }
    }

    protected AppSrvResult AppSrvResult() => new();

    protected AppSrvResult<TValue> AppSrvResult<TValue>(TValue value)
    {
        Checker.Argument.IsNotNull(value, nameof(value));
        return new AppSrvResult<TValue>(value);
    }

    protected ProblemDetails Problem(HttpStatusCode? statusCode = null, string? detail = null, string? title = null, string? instance = null, string? type = null) => new(statusCode, detail, title, instance, type);

    protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions) => expressions;
}