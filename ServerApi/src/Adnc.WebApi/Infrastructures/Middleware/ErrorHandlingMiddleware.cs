using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adnc.WebApi.Infrastructures.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            await next(context);

            var statusCode = context.Response.StatusCode;
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
        }

        //异常错误信息捕获，将错误信息用Json方式返回
        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {
            var result = JsonConvert.SerializeObject(new { message = msg });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }

    }

    //扩展方法
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
