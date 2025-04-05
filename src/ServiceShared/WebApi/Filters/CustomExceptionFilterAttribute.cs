using Adnc.Infra.Core.Exceptions;

namespace Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// 异常拦截器 拦截 StatusCode>=500 的异常
/// </summary>
public sealed class CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger, IWebHostEnvironment env) : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var status = 500;
        var exception = context.Exception;
        var requestId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        var eventId = new EventId(exception.HResult, requestId);
        var userContext = context.HttpContext.RequestServices.GetService<UserContext>();
        // var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //string className = descriptor.ControllerName;
        //string method = descriptor.ActionName;
        var hostAndPort = context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : string.Empty;
        var requestUrl = string.Concat(hostAndPort, context.HttpContext.Request.Path);
        var type = string.Concat("https://httpstatuses.com/", status);

        string title;
        string detial;
        if (exception is IAdncException adncException)
        {
            title = "业务异常";
            status = adncException.Status;
            detial = exception.Message;
        }
        else
        {
            title = env.IsDevelopment() ? exception.Message : $"系统异常";
            detial = env.IsDevelopment() ? exception.GetDetail() : $"系统异常,请联系管理员({eventId})";
            logger.LogError(eventId, exception, "{userId}:{requestUrl}", userContext?.Id, requestUrl);
        }
        var problemDetails = new ProblemDetails { Title = title, Detail = detial, Type = type, Status = status };

        context.Result = new ObjectResult(problemDetails) { StatusCode = status };
        context.ExceptionHandled = true;
    }

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}
