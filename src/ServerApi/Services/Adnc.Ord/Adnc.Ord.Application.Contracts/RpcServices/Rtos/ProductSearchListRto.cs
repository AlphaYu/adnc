using Refit;

namespace Adnc.Ord.Application.Contracts.RpcServices.Rtos
{
    public class ProductSearchListRto
    {
        [Query(CollectionFormat.Multi)]
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}