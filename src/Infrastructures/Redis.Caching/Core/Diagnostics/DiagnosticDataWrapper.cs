namespace Adnc.Infra.Redis.Caching.Core.Diagnostics
{
    public class DiagnosticDataWrapper
    {
        public Guid OperationId { get; set; }

        public long Timestamp { get; set; }
    }

    public class DiagnosticExceptionWrapper : DiagnosticDataWrapper
    {
        public Exception Exception { get; set; } = default!;
    }

    public class DiagnosticDataWrapper<T> : DiagnosticDataWrapper
        where T : notnull
    {
        public T EventData { get; set; } = default!;
    }
}