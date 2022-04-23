namespace Adnc.Shared.RpcServices.Services;

public interface IWhseRpcService : IRpcService
{
    /// <summary>
    /// <whse服务>获取商品列表
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 1000")]
    [Get("/whse/products")]
    Task<List<ProductRto>> GetProductsAsync(ProductSearchListRto search, CancellationToken cancellationToken = default);
}