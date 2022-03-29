using Adnc.Cus.Application.EventSubscribers;
using Adnc.Shared.Application.Contracts.Services;
using Adnc.Shared.RpcServices.Services;
using Microsoft.Extensions.Hosting;

namespace Adnc.Cus.Application
{
    public class AdncCusServiceCollection : AdncServiceCollection, IAdncServiceCollection
    {
        public AdncCusServiceCollection(IServiceCollection services)
        : base(services)
        {
        }

        public override void AddAdncServices()
        {
            AddEfCoreContext();

            AddMongoContext();

            var policies = GenerateDefaultRefitPolicies();
            var authServeiceAddress = _environment.IsDevelopment() ? "http://localhost:5010" : "adnc.usr.webapi";
            var maintServiceAddress = _environment.IsDevelopment() ? "http://localhost:5020" : "adnc.maint.webapi";
            AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
            AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

            AddEventBusSubscribers<CapEventSubscriber>();
        }
    }
}