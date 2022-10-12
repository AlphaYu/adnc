namespace Adnc.Infra.Redis.Caching.Core.Diagnostics
{
    public class DiagnosticDataWrapper
    {
        public Guid OperationId { get; set; }

        public long Timestamp { get; set; }
    }

    public class DiagnosticExceptionWrapper : DiagnosticDataWrapper
    {
        public Exception Exception { get; set; }
    }

    public class DiagnosticDataWrapper<T> : DiagnosticDataWrapper
    {
        public T EventData { get; set; }
    }
}