using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adnc.Core.Shared.Events
{
    public interface IEventPublisher
    {
        public Task PublishAsync<T>(T eventObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEvent;

        public Task PublishAsync<T>(T eventObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEvent;

        public void Publish<T>(T eventObj, string callbackName = null)
            where T : BaseEvent;

        public void Publish<T>(T eventObj, IDictionary<string, string> headers)
            where T : BaseEvent;
    }
}
