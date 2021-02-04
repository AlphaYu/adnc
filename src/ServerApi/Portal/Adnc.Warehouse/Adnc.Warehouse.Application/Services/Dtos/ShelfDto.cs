using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ShelfDto : BaseDto
    {
        public string Id { get; set; }
        public string ProductId { set; get; }
        public int Qty { set; get; }
        public int FreezedQty { set; get; }
        public ShelfPositionDto Position { get; set; }

        public string ProductSku { get; set; }

        public string ProductName { get; set; }
    }

    public class ShelfPositionDto
    {
        public string Code { get; set; }
        public string Description { get; set; }

    }
}
