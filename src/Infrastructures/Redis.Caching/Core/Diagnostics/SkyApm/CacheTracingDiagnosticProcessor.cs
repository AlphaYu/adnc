using SkyApm;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

namespace Adnc.Infra.Redis.Caching.Core.Diagnostics.SkyApm;

public sealed class CacheTracingDiagnosticProcessor(
    ITracingContext tracingContext,
    ILocalSegmentContextAccessor localSegmentContextAccessor,
    //IEntrySegmentContextAccessor entrySegmentContextAccessor,
    //IExitSegmentContextAccessor exitSegmentContextAccessor,
    IConfigAccessor configAccessor) : ITracingDiagnosticProcessor
{
    public static readonly StringOrIntValue Caching = new(nameof(Adnc.Infra.Redis.Caching));
    private readonly TracingConfig _tracingConfig = configAccessor.Get<TracingConfig>();

    public string ListenerName => CachingDiagnosticListenerExtensions.DiagnosticListenerName;

    #region GetCache

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingBeforeGetCache)]
    public void CachingBeforeGetCache([Object] DiagnosticDataWrapper<BeforeGetRequestEventData> eventData)
    {
        var context = tracingContext.CreateLocalSegmentContext("Cache: " + eventData.EventData.Operation);
        context.Span.SpanLayer = SpanLayer.CACHE;
        context.Span.Component = Caching;
        context.Span.AddTag("Name", eventData.EventData.Name);
        context.Span.AddTag("CacheType", eventData.EventData.CacheType);
        context.Span.AddTag(Tags.PATH, string.Join(",", eventData.EventData.CacheKeys));
        context.Span.AddLog(LogEvent.Event("Get Cache Start"));
        context.Span.AddLog(LogEvent.Message("Adnc.Caching get cache start..."));
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingAfterGetCache)]
    public void CachingAfterGetCache([Object] DiagnosticDataWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Get Cache End"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching get cache succeeded!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));

        tracingContext.Release(context);
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingErrorGetCache)]
    public void CachingErrorGetCache([Object] DiagnosticExceptionWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Get Cache Error"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching get cache failed!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));
        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);

        tracingContext.Release(context);
    }

    #endregion GetCache

    #region SetCache

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingBeforeSetCache)]
    public void CachingBeforeSetCache([Object] DiagnosticDataWrapper<BeforeSetRequestEventData> eventData)
    {
        var context = tracingContext.CreateLocalSegmentContext("Cache: " + eventData.EventData.Operation);
        context.Span.SpanLayer = SpanLayer.CACHE;
        context.Span.Component = Caching;
        context.Span.AddTag("Name", eventData.EventData.Name);
        context.Span.AddTag("CacheType", eventData.EventData.CacheType);
        context.Span.AddTag(Tags.PATH, string.Join(",", eventData.EventData.Dict.Keys));
        context.Span.AddLog(LogEvent.Event("Set Cache  Start"));
        context.Span.AddLog(LogEvent.Message("Adnc.Caching Set cache start..."));
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingAfterSetCache)]
    public void CachingAfterSetCache([Object] DiagnosticDataWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Set Cache End"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching set cache succeeded!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));

        tracingContext.Release(context);
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingErrorSetCache)]
    public void CachingErrorSetCache([Object] DiagnosticExceptionWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Set Cache Error"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching set cache failed!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));
        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);

        tracingContext.Release(context);
    }

    #endregion SetCache

    #region RemoveCache

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingBeforeRemoveCache)]
    public void CachingBeforeRemoveCache([Object] DiagnosticDataWrapper<BeforeRemoveRequestEventData> eventData)
    {
        var context = tracingContext.CreateLocalSegmentContext("Cache: " + eventData.EventData.Operation);
        context.Span.SpanLayer = SpanLayer.CACHE;
        context.Span.Component = Caching;
        context.Span.AddTag("Name", eventData.EventData.Name);
        context.Span.AddTag("CacheType", eventData.EventData.CacheType);
        context.Span.AddTag(Tags.PATH, string.Join(",", eventData.EventData.CacheKeys));
        context.Span.AddLog(LogEvent.Event("Remove Cache  Start"));
        context.Span.AddLog(LogEvent.Message("Adnc.Caching Remove cache start..."));
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingAfterRemoveCache)]
    public void CachingAfterRemoveCache([Object] DiagnosticDataWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Remove Cache End"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching remove cache succeeded!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));

        tracingContext.Release(context);
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingErrorRemoveCache)]
    public void CachingErrorRemoveCache([Object] DiagnosticExceptionWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Remove Cache Error"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching remove cache failed!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));
        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);

        tracingContext.Release(context);
    }

    #endregion RemoveCache

    #region ExistsCache

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingBeforeExistsCache)]
    public void CachingBeforeExistsCache([Object] DiagnosticDataWrapper<BeforeExistsRequestEventData> eventData)
    {
        var context = tracingContext.CreateLocalSegmentContext("Cache: " + eventData.EventData.Operation);
        context.Span.SpanLayer = SpanLayer.CACHE;
        context.Span.Component = Caching;
        context.Span.AddTag("Name", eventData.EventData.Name);
        context.Span.AddTag("CacheType", eventData.EventData.CacheType);
        context.Span.AddTag(Tags.PATH, eventData.EventData.CacheKey);
        context.Span.AddLog(LogEvent.Event("Exists Cache  Start"));
        context.Span.AddLog(LogEvent.Message("Adnc.Caching exists cache start..."));
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingAfterExistsCache)]
    public void CachingAfterExistsCache([Object] DiagnosticDataWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Exists Cache End"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching exists cache succeeded!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));

        tracingContext.Release(context);
    }

    [DiagnosticName(CachingDiagnosticListenerExtensions.CachingErrorExistsCache)]
    public void CachingErrorExistsCache([Object] DiagnosticExceptionWrapper eventData)
    {
        var context = localSegmentContextAccessor.Context;
        if (context == null)
        {
            return;
        }

        context.Span.AddLog(LogEvent.Event("Exists Cache Error"));
        context.Span.AddLog(LogEvent.Message($"Adnc.Caching exists cache failed!{Environment.NewLine}" +
                                             $"--> Message Id: {eventData.OperationId}"));
        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);

        tracingContext.Release(context);
    }

    #endregion ExistsCache
}
