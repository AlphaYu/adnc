using Adnc.Infra.Caching.Core.Diagnostics;
using SkyApm;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Diagnostics;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System;
using System.Collections.Concurrent;

namespace Adnc.Application.Shared.Caching
{
    public class CacheTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {
        public static readonly StringOrIntValue Caching = new StringOrIntValue(1000, "Adnc.Caching");
        private readonly ITracingContext _tracingContext;
        private readonly IEntrySegmentContextAccessor _entrySegmentContextAccessor;
        private readonly IExitSegmentContextAccessor _exitSegmentContextAccessor;
        private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;
        private readonly TracingConfig _tracingConfig;

        public string ListenerName => CachingDiagnosticListenerExtensions.DiagnosticListenerName;

        public CacheTracingDiagnosticProcessor(ITracingContext tracingContext,
            ILocalSegmentContextAccessor localSegmentContextAccessor
            , IEntrySegmentContextAccessor entrySegmentContextAccessor
            , IExitSegmentContextAccessor exitSegmentContextAccessor
            , IConfigAccessor configAccessor)
        {
            _tracingContext = tracingContext;
            _exitSegmentContextAccessor = exitSegmentContextAccessor;
            _localSegmentContextAccessor = localSegmentContextAccessor;
            _entrySegmentContextAccessor = entrySegmentContextAccessor;
            _tracingConfig = configAccessor.Get<TracingConfig>();
        }

        [DiagnosticName(CachingDiagnosticListenerExtensions.CachingBeforeGetCache)]
        public void CachingBeforeGetCache([Object] DiagnosticDataWrapper<BeforeGetRequestEventData> eventData)
        {
            var context = _tracingContext.CreateLocalSegmentContext("Cache: " + eventData.EventData.Operation);
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
            var context = _localSegmentContextAccessor.Context;
            if (context == null) return;

            context.Span.AddLog(LogEvent.Event("Get Cache End"));
            context.Span.AddLog(LogEvent.Message($"Adnc.Caching get cache succeeded!{Environment.NewLine}" +
                                                 $"--> Message Id: { eventData.OperationId }"));

            _tracingContext.Release(context);
        }

        [DiagnosticName(CachingDiagnosticListenerExtensions.CachingErrorGetCache)]
        public void CachingErrorGetCache([Object] DiagnosticExceptionWrapper eventData)
        {
            var context = _localSegmentContextAccessor.Context;
            if (context == null) return;

            context.Span.AddLog(LogEvent.Event("Get Cache Error"));
            context.Span.AddLog(LogEvent.Message($"Adnc.Caching get cache failed!{Environment.NewLine}" +
                                                 $"--> Message Id: { eventData.OperationId }"));
            context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);

            _tracingContext.Release(context);
        }
    }
}