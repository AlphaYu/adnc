using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Application.Dtos
{
    public class ShelfAllocateToProductDto : IDto
    {
        public string ProductId { get; set; }
    }
}
