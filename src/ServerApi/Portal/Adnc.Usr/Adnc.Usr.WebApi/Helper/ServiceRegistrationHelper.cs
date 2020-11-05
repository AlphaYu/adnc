using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Adnc.WebApi.Shared;

namespace Adnc.Usr.WebApi.Helper
{
    public sealed class ServiceRegistrationHelper : SharedServicesRegistration
    {
        public ServiceRegistrationHelper(IConfiguration configuration
            , IServiceCollection services
            , IWebHostEnvironment env
            , ServiceInfo serviceInfo)
          : base(configuration, services, env, serviceInfo)
        {

        }
    }
}
