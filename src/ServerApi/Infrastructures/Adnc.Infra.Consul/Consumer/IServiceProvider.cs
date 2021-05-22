using System.Collections.Generic;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public interface IServiceProvider
    {
        Task<IList<string>> GetServicesAsync(string serviceName);
    }
}