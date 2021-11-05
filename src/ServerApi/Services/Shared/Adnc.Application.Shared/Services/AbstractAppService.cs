using Adnc.Infra.Mapper;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Net;

namespace Adnc.Application.Shared.Services
{
    public abstract class AbstractAppService : IAppService
    {
        public IObjectMapper Mapper { get; set; }

        protected AppSrvResult AppSrvResult()
            => new AppSrvResult();

        protected AppSrvResult<TValue> AppSrvResult<TValue>([NotNull] TValue value)
            => new AppSrvResult<TValue>(value);

        protected ProblemDetails Problem(HttpStatusCode? statusCode = null, string detail = null, string title = null, string instance = null, string type = null)
            => new ProblemDetails(statusCode, detail, title, instance, type);

        protected Expression<Func<TEntity, object>>[] UpdatingProps<TEntity>(params Expression<Func<TEntity, object>>[] expressions)
            => expressions;
    }
}