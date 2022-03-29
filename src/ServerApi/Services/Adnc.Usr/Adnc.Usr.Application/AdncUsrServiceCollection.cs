namespace Adnc.Shared.Application
{
    public class AdncUsrServiceCollection : AdncServiceCollection
    {
        public AdncUsrServiceCollection(IServiceCollection services)
        : base(services)
        {
        }

        public override void AddAdncServices()
        {
            AddEfCoreContext();
            AddMongoContext();
        }
    }
}