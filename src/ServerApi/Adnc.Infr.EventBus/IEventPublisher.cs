using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Infr.EventBus
{
    public interface IEventPublisher
    {
        public Task PublishAsync<T>(T eventObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : IEvent;

        public Task PublishAsync<T>(T eventObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
            where T : IEvent;

        public void Publish<T>(T eventObj, string callbackName = null)
            where T : IEvent;

        public void Publish<T>(T eventObj, IDictionary<string, string> headers)
            where T : IEvent;
    }
}
