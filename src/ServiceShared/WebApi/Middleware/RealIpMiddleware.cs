namespace Adnc.Shared.WebApi.Middleware;

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
        if (_option.HeaderKey.IsNullOrWhiteSpace())
        {
            await _next(context);
            return;
        }

        var ips = context.Request.Headers[_option.HeaderKey].FirstOrDefault()?.Trim();
        if (string.IsNullOrEmpty(ips))
        {
            await _next(context);
            return;
        }

        var realIp = ips.Split(",")[0];
        if (realIp == string.Empty)
        {
            await _next(context);
            return;
        }

        context.Connection.RemoteIpAddress = IPAddress.Parse(realIp);
        _logger.LogDebug($"Resolve real ip success: {context.Connection.RemoteIpAddress}");

        await _next(context);
    }
}

public class FilterOption
{
    public string HeaderKey { get; set; } = string.Empty;
}

public static class RealIpMiddlewareExtensions
{
    public static IApplicationBuilder UseRealIp(this IApplicationBuilder builder, Action<FilterOption>? configureOption = null)
    {
        var option = new FilterOption();
        configureOption?.Invoke(option);
        return builder.UseMiddleware<RealIpMiddleware>(option);
    }
}