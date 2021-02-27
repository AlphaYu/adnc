using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Ord.Application.RpcServices.Rtos
{
    public class ProductSearchListRto
    {
        [Query(CollectionFormat.Multi)]
        public string[] Ids { get; set; }

        public int StatusCode { get; set; }
    }
}
