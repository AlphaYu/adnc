namespace Adnc.Infra.Redis.Caching.Core.Diagnostics
{
    /// <summary>
    /// Extension methods on the DiagnosticListener class to log Adnc.Infra.Redis
    /// </summary>
    public static class CachingDiagnosticListenerExtensions
    {
        public const string DiagnosticListenerName = "CachingDiagnosticListener";

        private const string CachingPrefix = nameof(Adnc.Infra.Redis.Caching);

        public const string CachingBeforeSetCache = CachingPrefix + nameof(WriteSetCacheBefore);
        public const string CachingAfterSetCache = CachingPrefix + nameof(WriteSetCacheAfter);
        public const string CachingErrorSetCache = CachingPrefix + nameof(WriteSetCacheError);

        public const string CachingBeforeRemoveCache = CachingPrefix + nameof(WriteRemoveCacheBefore);
        public const string CachingAfterRemoveCache = CachingPrefix + nameof(WriteRemoveCacheAfter);
        public const string CachingErrorRemoveCache = CachingPrefix + nameof(WriteRemoveCacheError);

        public const string CachingBeforeGetCache = CachingPrefix + nameof(WriteGetCacheBefore);
        public const string CachingAfterGetCache = CachingPrefix + nameof(WriteGetCacheAfter);
        public const string CachingErrorGetCache = CachingPrefix + nameof(WriteGetCacheError);

        public const string CachingBeforeExistsCache = CachingPrefix + nameof(WriteExistsCacheBefore);
        public const string CachingAfterExistsCache = CachingPrefix + nameof(WriteExistsCacheAfter);
        public const string CachingErrorExistsCache = CachingPrefix + nameof(WriteExistsCacheError);

        public const string CachingBeforeFlushCache = CachingPrefix + nameof(WriteFlushCacheBefore);
        public const string CachingAfterFlushCache = CachingPrefix + nameof(WriteFlushCacheAfter);
        public const string CachingErrorFlushCache = CachingPrefix + nameof(WriteFlushCacheError);

        public const string CachingBeforePublishMessage = CachingPrefix + nameof(WritePublishMessageBefore);
        public const string CachingAfterPublishMessage = CachingPrefix + nameof(WritePublishMessageAfter);
        public const string CachingErrorPublishMessage = CachingPrefix + nameof(WritePublishMessageError);

        public const string CachingBeforeSubscribeMessage = CachingPrefix + nameof(WriteSubscribeMessageBefore);
        public const string CachingAfterSubscribeMessage = CachingPrefix + nameof(WriteSubscribeMessageAfter);
        public const string CachingErrorSubscribeMessage = CachingPrefix + nameof(WriteSubscribeMessageError);

        public static void WriteSetCacheError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorSetCache))
            {
                @this.Write(CachingErrorSetCache, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteRemoveCacheError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorRemoveCache))
            {
                @this.Write(CachingErrorRemoveCache, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteGetCacheError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorGetCache))
            {
                @this.Write(CachingErrorGetCache, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteExistsCacheError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorExistsCache))
            {
                @this.Write(CachingErrorExistsCache, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteFlushCacheError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorFlushCache))
            {
                @this.Write(CachingErrorFlushCache, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WritePublishMessageError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorPublishMessage))
            {
                @this.Write(CachingErrorPublishMessage, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteSubscribeMessageError(this DiagnosticListener @this, Guid operationId, Exception ex)
        {
            if (@this.IsEnabled(CachingErrorSubscribeMessage))
            {
                @this.Write(CachingErrorSubscribeMessage, new DiagnosticExceptionWrapper
                {
                    OperationId = operationId,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteSetCacheAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterSetCache))
            {
                @this.Write(CachingAfterSetCache, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteRemoveCacheAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterRemoveCache))
            {
                @this.Write(CachingAfterRemoveCache, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteGetCacheAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterGetCache))
            {
                @this.Write(CachingAfterGetCache, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteExistsCacheAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterExistsCache))
            {
                @this.Write(CachingAfterExistsCache, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteFlushCacheAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterFlushCache))
            {
                @this.Write(CachingAfterFlushCache, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WritePublishMessageAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterPublishMessage))
            {
                @this.Write(CachingAfterPublishMessage, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WriteSubscribeMessageAfter(this DiagnosticListener @this, Guid operationId)
        {
            if (@this.IsEnabled(CachingAfterSubscribeMessage))
            {
                @this.Write(CachingAfterSubscribeMessage, new DiagnosticDataWrapper
                {
                    OperationId = operationId,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static Guid WriteSetCacheBefore(this DiagnosticListener @this, BeforeSetRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeSetCache))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforeSetCache, new DiagnosticDataWrapper<BeforeSetRequestEventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WriteRemoveCacheBefore(this DiagnosticListener @this, BeforeRemoveRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeRemoveCache))
            {
                Guid operationId = Guid.NewGuid();
                @this.Write(CachingBeforeRemoveCache, new DiagnosticDataWrapper<BeforeRemoveRequestEventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });
                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WriteGetCacheBefore(this DiagnosticListener @this, BeforeGetRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeGetCache))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforeGetCache, new DiagnosticDataWrapper<BeforeGetRequestEventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WriteExistsCacheBefore(this DiagnosticListener @this, BeforeExistsRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeExistsCache))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforeExistsCache, new DiagnosticDataWrapper<BeforeExistsRequestEventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WriteFlushCacheBefore(this DiagnosticListener @this, EventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeFlushCache))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforeFlushCache, new DiagnosticDataWrapper<EventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WriteSubscribeMessageBefore(this DiagnosticListener @this, BeforeSubscribeMessageRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforeSubscribeMessage))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforeSubscribeMessage, new DiagnosticDataWrapper<BeforeSubscribeMessageRequestEventData>
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static Guid WritePublishMessageBefore(this DiagnosticListener @this, BeforePublishMessageRequestEventData eventData)
        {
            if (@this.IsEnabled(CachingBeforePublishMessage))
            {
                Guid operationId = Guid.NewGuid();

                @this.Write(CachingBeforePublishMessage, new
                {
                    OperationId = operationId,
                    EventData = eventData,
                    Timestamp = Stopwatch.GetTimestamp()
                });

                return operationId;
            }

            return Guid.Empty;
        }
    }
}