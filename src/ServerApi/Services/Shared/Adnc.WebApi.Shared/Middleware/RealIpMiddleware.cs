using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
                        var ips = headers[headerKey].FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(ips))
                        {
                            var realIp = ips.Split(",", StringSplitOptions.RemoveEmptyEntries)[0];
                            context.Connection.RemoteIpAddress = IPAddress.Parse(realIp);
                            _logger.LogDebug($"Resolve real ip success: {context.Connection.RemoteIpAddress}");
                            break;
                        }
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
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