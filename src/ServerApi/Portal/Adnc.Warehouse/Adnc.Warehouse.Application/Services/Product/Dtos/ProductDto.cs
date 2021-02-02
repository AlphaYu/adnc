using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ProductDto : BaseDto
    {
        public long ID { set; get; }

        public string Sku { set; get; }

        public string Name { set; get; }

        public string Describe { set; get; }

        public float Price { set; get; }

        public int Status { set; get; }

        public string StatusName { get; set; }

        public string ChangeStatusReason { get; set; }

        public long AssignedWarehouseId { set; get; }

        public string Unit { set; get; }

    }
}
