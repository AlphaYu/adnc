using Refit;

namespace Adnc.Application.RpcService.Rtos
{
    public class ProductSearchListRto
    {
        [Query(CollectionFormat.Multi)]
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}