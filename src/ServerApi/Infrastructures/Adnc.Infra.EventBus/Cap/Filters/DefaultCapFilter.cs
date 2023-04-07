using DotNetCore.CAP.Filter;

namespace Adnc.Infra.EventBus.Cap.Filters;

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

    public override void OnSubscribeExecuting(ExecutingContext context)
    {
    }

    public override void OnSubscribeExecuted(ExecutedContext context)
    {
    }

    public override void OnSubscribeException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);
    }
}