using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNetCore.CAP;

namespace Adnc.Core.Shared.Events
{
    public interface IEventPublisher
    {
        public Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEto;

        public Task PublishAsync<T>(string name, T contentObj, IDictionary<string, string> headers, CancellationToken cancellationToken = default(CancellationToken))
            where T : BaseEto;

        public void Publish<T>(string name, T contentObj, string callbackName = null)
            where T : BaseEto;

        public void Publish<T>(string name, T contentObj, IDictionary<string, string> headers)
            where T : BaseEto;
    }
}