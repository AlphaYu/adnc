using DotNetCore.CAP;

namespace Adnc.Infra.EventBus.Cap
{
    public class CapPublisher(ICapPublisher publisher) : IEventPublisher
    {
        public async Task PublishAsync<T>(T contentObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class
            => await publisher.PublishAsync(typeof(T).Name, contentObj, callbackName, cancellationToken);

        public async Task PublishAsync<T>(T contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class
            => await publisher.PublishAsync<T>(typeof(T).Name, contentObj, headers, cancellationToken);

        public void Publish<T>(T contentObj, string? callbackName = null) where T : class
            => publisher.Publish(typeof(T).Name, contentObj, callbackName);

        public void Publish<T>(T contentObj, IDictionary<string, string?> headers) where T : class
            => publisher.Publish(typeof(T).Name, contentObj, headers);

        public async Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default)
            => await publisher.PublishDelayAsync(delayTime, typeof(T).Name, contentObj, headers, cancellationToken);

        public async Task PublishDelayAsync<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null, CancellationToken cancellationToken = default)
            => await publisher.PublishDelayAsync(delayTime, typeof(T).Name, contentObj, callbackName, cancellationToken);

        public void PublishDelay<T>(TimeSpan delayTime, T? contentObj, IDictionary<string, string?> headers)
            => publisher.PublishDelay(delayTime, typeof(T).Name, contentObj, headers);

        public void PublishDelay<T>(TimeSpan delayTime, T? contentObj, string? callbackName = null)
            => publisher.PublishDelay(delayTime, typeof(T).Name, contentObj, callbackName);
    }
}