using Refit;

namespace Adnc.Shared.RpcService.Rtos
{
    public class ProductSearchListRto
    {
        [Query(CollectionFormat.Multi)]
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}