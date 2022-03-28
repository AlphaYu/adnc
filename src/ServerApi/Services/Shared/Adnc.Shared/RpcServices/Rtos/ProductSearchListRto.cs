namespace Adnc.Shared.RpcServices.Rtos;

public class ProductSearchListRto
{
    [Query(CollectionFormat.Multi)]
    public long[] Ids { get; set; }

    public int StatusCode { get; set; }
}