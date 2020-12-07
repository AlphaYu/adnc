using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Adnc.WebApi.Shared.Middleware
{
    public class RealIpMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<RealIpMiddleware> _logger;
        private readonly FilterOption _option;

        public RealIpMiddleware(RequestDelegate next, ILogger<RealIpMiddleware> logger, FilterOption option)
        {
            _next = next;
            _logger = logger;
            _option = option;
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            try
            {
                if (_option.HeaderKeys != null && _option.HeaderKeys.Length > 0)
                {
                    foreach (var headerKey in _option.HeaderKeys)
                    {
                        if (headers.ContainsKey(headerKey))
                        {
                            context.Connection.RemoteIpAddress = IPAddress.Parse(headers[headerKey].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0]);
                            _logger.LogDebug($"Resolve real ip success: {context.Connection.RemoteIpAddress}");
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                await _next(context);
            }
        }
    }

    public class FilterOption
    {
        public string[] HeaderKeys { get; set; }
    }

    public static class RealIpMiddlewareExtensions
    {
        public static IApplicationBuilder UseRealIp(this IApplicationBuilder builder, Action<FilterOption> configureOption = null)
        {
            var option = new FilterOption();
            configureOption?.Invoke(option);
            return builder.UseMiddleware<RealIpMiddleware>(option);
        }
    }
}
