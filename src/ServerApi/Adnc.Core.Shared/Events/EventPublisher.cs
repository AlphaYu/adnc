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

        public virtual async Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEto
        {
            await _eventBus.PublishAsync(name, contentObj, callbackName, cancellationToken);
        }

        public virtual async Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEto
        {
            await _eventBus.PublishAsync<T>(name, contentObj, headers, cancellationToken);
        }

        public virtual void Publish<T>(string name, T contentObj, string callbackName = null)
            where T : BaseEto
        {
            _eventBus.Publish(name, contentObj, callbackName);
        }

        public virtual void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
            where T : BaseEto
        {
            _eventBus.Publish(name, contentObj, headers);
        }
    }
}
