using System.Net;
using JetBrains.Annotations;
using AutoMapper;

namespace Adnc.Application.Shared.Services
{
    public abstract class AppService : IAppService
    {
        protected IMapper Mapper { get; set; }

        protected AppSrvResult DefaultResult()
        {
            return new AppSrvResult();
        }

        protected AppSrvResult<TValue> OkReulst<TValue>([NotNull] TValue value)
        {
            return new AppSrvResult<TValue>(value);
        }

        protected ProblemDetails Problem(HttpStatusCode? statusCode = null, string detail = null, string title = null, string instance = null, string type = null)
        {
            return new ProblemDetails(statusCode, detail, title, instance, type);
        }
    }
}
