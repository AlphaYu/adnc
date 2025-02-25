namespace Adnc.Infra.Consul.Discover;

public interface IDiscoverProvider
{
    Task<IList<string>> GetAllHealthServicesAsync();

    Task<string> GetSingleHealthServiceAsync();
}