using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Adnc.Infr.Common;
using Adnc.Application.Shared;
using Adnc.Infr.Common.Helper;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            Exception exception = context.Exception;
            JsonResult result = null;
            if (exception is BusinessException)
            {
                result = new JsonResult(
                    JsonSerializer.Deserialize<ErrorModel>(exception.Message, SystemTextJsonHelper.GetAdncDefaultOptions())
                    , SystemTextJsonHelper.GetAdncDefaultOptions())
                {
                    StatusCode = exception.HResult
                };
            }
            else
            {
                result = new JsonResult(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, "服务器异常")
                                        , SystemTextJsonHelper.GetAdncDefaultOptions());

                var userContext = context.HttpContext.RequestServices.GetService<UserContext>();

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                string className = descriptor.ControllerName;
                string method = descriptor.ActionName;
                string requestUrl = context.HttpContext.Request.Path;
                long userId = userContext.ID;
                //var parms = ex.Data?.ToDictionary().Select(k => k.Key + "=" + k.Value).Join() ?? "";

                _logger.LogError(exception, exception.Message);
                //Agent.Tracer.CurrentTransaction.CaptureException(exception);
            }

            context.Result = result;
            context.ExceptionHandled = true;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}