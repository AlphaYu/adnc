using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace Adnc.Shared.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<CustomExceptionHandlerMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            var envVariable = (Environment.GetEnvironmentVariable("DEMO_SERVER") ?? "false").ToLower();
            if (envVariable == "false")
            {
                await next(context);
            }
            else
            {
                string[] methods = ["post", "put", "delete", "patch"];
                var curretMethod = context.Request.Method.ToLower();
                if (!methods.Contains(curretMethod))
                {
                    await next(context);
                }
                else
                {
                    var path = context.Request.Path.Value;
                    if (path is null || path.Contains("auth/session", StringComparison.CurrentCultureIgnoreCase)
                        || path.Contains("profiler/results", StringComparison.CurrentCultureIgnoreCase))
                    {
                        await next(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/problem+json";
                        var type = string.Concat("https://httpstatuses.com/", context.Response.StatusCode);
                        var title = "演示环境";
                        var detial = "演示环境,不允许操作";
                        var problemDetails = new ProblemDetails { Title = title, Detail = detial, Type = type, Status = context.Response.StatusCode };
                        var errorText = JsonSerializer.Serialize(problemDetails, SystemTextJson.GetAdncDefaultOptions());
                        await context.Response.WriteAsync(errorText);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var requestId = System.Diagnostics.Activity.Current?.Id ?? context.TraceIdentifier;
        var eventId = new EventId(exception.HResult, requestId);
        logger.LogError(eventId, exception, "CustomExceptionHandlerMiddleware.HandleExceptionAsync");

        var status = 500;
        var type = string.Concat("https://httpstatuses.com/", status);
        var title = env.IsDevelopment() ? exception.Message : $"系统异常";
        var detial = env.IsDevelopment() ? exception.GetDetail() : $"系统异常,请联系管理员({eventId})";

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";
        var problemDetails = new ProblemDetails { Title = title, Detail = detial, Type = type, Status = status };
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
