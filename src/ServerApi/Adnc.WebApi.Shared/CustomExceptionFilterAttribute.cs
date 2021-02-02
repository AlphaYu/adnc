using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Adnc.Infr.Common;
using Adnc.Infr.Common.Helper;

namespace Microsoft.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// 异常拦截器 拦截 StatusCode>=500 的异常
    /// </summary>
    public sealed class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilterAttribute> _logger;
        private readonly IWebHostEnvironment _env;

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger
            , IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public override void OnException(ExceptionContext context)
        {
            var status = 500;
            var exception = context.Exception;
            var eventId = new EventId(exception.HResult);
            var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //string className = descriptor.ControllerName;
            //string method = descriptor.ActionName;
            var hostAndPort = context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : string.Empty;
            var requestUrl = string.Concat(hostAndPort, context.HttpContext.Request.Path);
            var type = string.Concat("https://httpstatuses.com/", status);
            var title = _env.IsDevelopment() ? exception.Message : $"系统异常";
            var detial = _env.IsDevelopment() ? ExceptionHelper.GetExceptionDetail(exception) : $"系统异常,请联系管理员({eventId})";

            _logger.LogError(eventId, exception, exception.Message, requestUrl, userContext.Id);

            var problemDetails = new ProblemDetails
            {
                Title = title
                ,
                Detail = detial
                ,
                Type = type
                ,
                Status = status
                ,
                Instance = requestUrl
            };

            context.Result = new ObjectResult(problemDetails) { StatusCode = status };
            context.ExceptionHandled = true;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }


    }
}