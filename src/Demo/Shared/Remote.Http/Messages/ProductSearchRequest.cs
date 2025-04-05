namespace Adnc.Demo.Remote.Http.Messages;

public class ProductSearchRequest
{
    [Query(CollectionFormat.Multi)]
    public long[] Ids { get; set; } = [];

    public int StatusCode { get; set; }
}
