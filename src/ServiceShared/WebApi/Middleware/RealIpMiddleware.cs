namespace Adnc.Shared.WebApi.Middleware;

public class RealIpMiddleware(RequestDelegate next, ILogger<RealIpMiddleware> logger, FilterOption option)
{
    public async Task Invoke(HttpContext context)
    {
        if (option.HeaderKey.IsNullOrWhiteSpace())
        {
            await next(context);
            return;
        }

        var ips = context.Request.Headers[option.HeaderKey].FirstOrDefault()?.Trim();
        if (string.IsNullOrEmpty(ips))
        {
            await next(context);
            return;
        }

        var realIp = ips.Split(",")[0];
        if (realIp == string.Empty)
        {
            await next(context);
            return;
        }

        context.Connection.RemoteIpAddress = IPAddress.Parse(realIp);
        logger.LogDebug("Resolve real ip success: {realIp}", realIp);

        await next(context);
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
