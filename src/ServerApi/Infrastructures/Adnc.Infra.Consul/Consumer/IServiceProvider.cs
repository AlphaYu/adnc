using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adnc.Infra.Consul.Consumer
{
    public interface IServiceProvider
    {
        Task<IList<string>> GetServicesAsync(string serviceName);
    }
}
