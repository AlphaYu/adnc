namespace Adnc.Infra.EventBus
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T contentObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class;

        Task PublishAsync<T>(T contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class;

        void Publish<T>(T contentObj, string? callbackName = null) where T : class;

        void Publish<T>(T contentObj, IDictionary<string, string?> headers) where T : class;

        Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default);

        Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default);

        void PublishDelay<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers);

        void PublishDelay<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null);
    }
}