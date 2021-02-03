using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ShelfDto : BaseDto
    {
        public long Id { get; set; }
        public long? ProductId { protected set; get; }
        public int Qty { protected set; get; }
        public int FreezedQty { protected set; get; }
        public ShelfPositionDto Position { get; private set; }
    }

    public class ShelfPositionDto 
    {
        public string Code { get; set; }
        public string Description { get; set; }

    }
}
