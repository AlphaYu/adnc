namespace Adnc.Infra.Consul.Consumer
{
    public interface IConsulServiceProvider
    {
        Task<IList<string>> GetHealthServicesAsync(string serviceName);
    }
}