using Adnc.Shared.RpcServices.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Adnc.Maint.Application
{
    public class AdncMaintServiceCollection : AdncServiceCollection
    {
        public AdncMaintServiceCollection(IServiceCollection services)
        : base(services)
        {
        }

        public override void AddAdncServices()
        {
            AddEfCoreContext();

            AddMongoContext();

            var policies = GenerateDefaultRefitPolicies();
            var authServeiceAddress = _environment.IsDevelopment() ? "http://localhost:5010" : "adnc.usr.webapi";
            AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        }
    }
}