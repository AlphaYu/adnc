namespace Adnc.Infra.EventBus
{
    public interface IEventPublisher
    {
        public Task PublishAsync<T>(T eventObj, string? callbackName = null, CancellationToken cancellationToken = default) where T : class;

        public Task PublishAsync<T>(T eventObj, IDictionary<string, string?> headers, CancellationToken cancellationToken = default) where T : class;

        public void Publish<T>(T eventObj, string? callbackName = null) where T : class;

        public void Publish<T>(T eventObj, IDictionary<string, string?> headers) where T : class;
    }
}