using Adnc.Shared.Application.Contracts.Services;
using Adnc.Whse.Application.EventSubscribers;
using Microsoft.Extensions.Hosting;

namespace Adnc.Whse.Application
{
    public class AdncWhseServiceCollection : AdncServiceCollection, IAdncServiceCollection
    {
        public AdncWhseServiceCollection(IServiceCollection services)
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

            var maintServiceAddress = _environment.IsDevelopment() ? "http://localhost:5020" : "adnc.maint.webapi";
            AddRpcService<IMaintRpcService>(maintServiceAddress, policies);

            AddEventBusSubscribers<CapEventSubscriber>();
        }
    }
}