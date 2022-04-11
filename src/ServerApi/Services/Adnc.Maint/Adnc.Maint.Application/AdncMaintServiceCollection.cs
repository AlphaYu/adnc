using Adnc.Shared.RpcServices.Services;

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
            var authServeiceAddress = IsDevelopment ? "http://localhost:5010" : "adnc.usr.webapi";
            AddRpcService<IAuthRpcService>(authServeiceAddress, policies);
        }
    }
}