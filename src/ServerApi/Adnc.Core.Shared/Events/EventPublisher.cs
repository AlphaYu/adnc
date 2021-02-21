using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace Adnc.Core.Shared.Events
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ICapPublisher _eventBus;

        public EventPublisher(ICapPublisher capPublisher)
        {
            _eventBus = capPublisher;
        }

        public virtual async Task PublishAsync<T>(T eventObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEvent
        {
            await _eventBus.PublishAsync(nameof(T), eventObj, callbackName, cancellationToken);
        }

        public virtual async Task PublishAsync<T>(T eventObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEvent
        {
            await _eventBus.PublishAsync<T>(nameof(T), eventObj, headers, cancellationToken);
        }

        public virtual void Publish<T>(T eventObj, string callbackName = null)
            where T : BaseEvent
        {
            _eventBus.Publish(nameof(T), eventObj, callbackName);
        }

        public virtual void Publish<T>(T eventObj, IDictionary<string, string> headers)
            where T : BaseEvent
        {
            _eventBus.Publish(nameof(T), eventObj, headers);
        }
    }
}
