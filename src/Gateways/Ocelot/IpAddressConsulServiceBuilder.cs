using Consul;
using Ocelot.Logging;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Consul.Interfaces;

namespace Adnc.Gateway.Ocelot;

public class IpAddressConsulServiceBuilder(IHttpContextAccessor contextAccessor, IConsulClientFactory clientFactory, IOcelotLoggerFactory loggerFactory)
    : DefaultConsulServiceBuilder(contextAccessor, clientFactory, loggerFactory)
{
    // I want to use the agent service IP address as the downstream hostname
    protected override string GetDownstreamHost(ServiceEntry entry, Node node)
        => entry.Service.Address;
}
