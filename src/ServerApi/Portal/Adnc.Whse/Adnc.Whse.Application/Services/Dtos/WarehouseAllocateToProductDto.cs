using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Whse.Application.Dtos
{
    public class WarehouseAllocateToProductDto : IDto
    {
        public string ProductId { get; set; }
    }
}
