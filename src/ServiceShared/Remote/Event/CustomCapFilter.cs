using DotNetCore.CAP.Filter;

namespace Adnc.Shared.Remote.Event;

/// <summary>
/// https://cap.dotnetcore.xyz/user-guide/zh/cap/filter/
/// </summary>
public sealed class DefaultCapFilter : SubscribeFilter
{
    private readonly ILogger<DefaultCapFilter> _logger;

    public DefaultCapFilter(ILogger<DefaultCapFilter> logger)
    {
        _logger = logger;
    }

    public override Task OnSubscribeExecutingAsync(ExecutingContext context)
    {
        return Task.CompletedTask;
    }

    public override Task OnSubscribeExecutedAsync(ExecutedContext context)
    {
        return Task.CompletedTask;
    }

    public override Task OnSubscribeExceptionAsync(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "DefaultCapFilter.OnSubscribeExceptionAsync");
        return Task.CompletedTask;
    }
}
