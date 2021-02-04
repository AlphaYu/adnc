using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Core.Shared.EventBus;

namespace Adnc.Warehouse.Core.EventBus.Etos
{
    public class ShelfToProductAllocatedEto : BaseEto
    {
        public long ShelfId { get; set; }

        public long ProductId { get; set; }
    }
}
