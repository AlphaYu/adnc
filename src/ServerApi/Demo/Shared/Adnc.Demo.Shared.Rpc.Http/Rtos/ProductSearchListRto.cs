namespace Adnc.Demo.Shared.Rpc.Http.Rtos;

public class ProductSearchListRto
{
    /// <summary>
    /// 构造函数
    /// 修复Warning, add by garfield 20220530
    /// </summary>
    public ProductSearchListRto()
    {
        Ids = Array.Empty<long>();
    }

    [Query(CollectionFormat.Multi)]
    public long[] Ids { get; set; }

    public int StatusCode { get; set; }
}