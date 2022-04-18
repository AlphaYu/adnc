namespace Adnc.Shared.RpcServices.Services;

public interface IWhseRpcService : IRpcService
{
    /// <summary>
    /// 获取商品列表
    /// </summary>
    /// <returns></returns>
    [Headers("Authorization: Basic", "Cache: 1000")]
    [Get("/whse/products")]
    Task<ApiResponse<List<ProductRto>>> GetProductsAsync(ProductSearchListRto search, CancellationToken cancellationToken = default);
}