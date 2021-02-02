using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace Adnc.Warehouse.Core.EventBus
{
    public class Publisher
    {
        private readonly ICapPublisher _eventBus;

        public Publisher(ICapPublisher capPublisher)
        {
            _eventBus = capPublisher;
        }

        public async Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _eventBus.PublishAsync<T>(name, contentObj, callbackName, cancellationToken);
        }

        public async Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _eventBus.PublishAsync<T>(name, contentObj, headers, cancellationToken);
        }

        public void Publish<T>(string name, T contentObj, string callbackName = null)
        {
            _eventBus.Publish(name, contentObj, callbackName);
        }

        public void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
        {
            _eventBus.Publish(name, contentObj, headers);
        }
    }
}
