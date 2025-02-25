using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Adnc.Shared.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;
    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next
        , IWebHostEnvironment env
        , ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }

        #region old code

        //var statusCode = context.Response.StatusCode;
        //string msg = string.Empty;
        //if (statusCode == 401)
        //{
        //    msg = "未授权";
        //}
        //else if (statusCode == 403)
        //{
        //    msg = "未授权，没有权限";
        //}
        //else if (statusCode == 404)
        //{
        //    msg = "未找到服务";
        //}

        //if (!string.IsNullOrWhiteSpace(msg))
        //{
        //    await HandleExceptionAsync(context, statusCode, msg);
        //}

        #endregion old code
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var requestId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier; 
        var eventId = new EventId( exception.HResult, requestId);
        _logger.LogError(eventId, exception, exception.Message);

        var status = 500;
        var type = string.Concat("https://httpstatuses.com/", status);
        var title = _env.IsDevelopment() ? exception.Message : $"系统异常";
        var detial = _env.IsDevelopment() ? exception.GetExceptionDetail() : $"系统异常,请联系管理员({eventId})";

        var problemDetails = new ProblemDetails
        {
            Title = title
            ,
            Detail = detial
            ,
            Type = type
            ,
            Status = status
        };

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var errorText = JsonSerializer.Serialize(problemDetails, SystemTextJson.GetAdncDefaultOptions());
        await context.Response.WriteAsync(errorText);
    }
}

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}