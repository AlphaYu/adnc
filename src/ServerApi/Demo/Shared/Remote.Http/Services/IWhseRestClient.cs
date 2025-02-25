using Adnc.Shared.Remote.Http;

namespace Adnc.Demo.Shared.Remote.Http.Services;

public interface IWhseRestClient : IRestClient
{
    /// <summary>
    /// <whse服务>获取商品列表
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 1000")]
    [Get("/whse/api/products")]
    Task<ApiResponse<List<ProductRto>>> GetProductsAsync(ProductSearchListRto search, CancellationToken cancellationToken = default);
}