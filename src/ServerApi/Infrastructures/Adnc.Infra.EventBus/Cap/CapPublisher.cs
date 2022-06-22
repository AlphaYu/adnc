using DotNetCore.CAP;

namespace Adnc.Infra.EventBus.Cap
{
    public class CapPublisher : IEventPublisher
    {
        private readonly ICapPublisher _eventBus;

        public CapPublisher(ICapPublisher capPublisher)
            => _eventBus = capPublisher;

        public virtual async Task PublishAsync<T>(T eventObj, string? callbackName = null, CancellationToken cancellationToken = default)
            where T : class
            => await _eventBus.PublishAsync(typeof(T).Name, eventObj, callbackName, cancellationToken);

        public virtual async Task PublishAsync<T>(T eventObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default)
            where T : class
            => await _eventBus.PublishAsync<T>(typeof(T).Name, eventObj, headers, cancellationToken);

        public virtual void Publish<T>(T eventObj, string? callbackName = null)
            where T : class
            => _eventBus.Publish(typeof(T).Name, eventObj, callbackName);

        public virtual void Publish<T>(T eventObj, IDictionary<string, string?> headers)
            where T : class
            => _eventBus.Publish(typeof(T).Name, eventObj, headers);
    }
}