using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Dtos
{
    public class WarehouseDto : IDto
    {
        public string Id { get; set; }

        public string ProductId { set; get; }

        public int Qty { set; get; }

        public int FreezedQty { set; get; }

        public string PositionCode { get; set; }

        public string PositionDescription { get; set; }

        public string ProductSku { get; set; }

        public string ProductName { get; set; }
    }
}
