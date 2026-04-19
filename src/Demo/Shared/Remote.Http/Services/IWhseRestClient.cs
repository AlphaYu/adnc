using Adnc.Shared.Remote.Http;

namespace Adnc.Demo.Remote.Http.Services;

public interface IWhseRestClient : IRestClient
{
    /// <summary>
    /// Get Products List
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 1000")]
    [Get("/whse/api/products")]
    Task<List<ProductResponse>> GetProductsAsync(ProductSearchRequest input, CancellationToken cancellationToken = default);
}
