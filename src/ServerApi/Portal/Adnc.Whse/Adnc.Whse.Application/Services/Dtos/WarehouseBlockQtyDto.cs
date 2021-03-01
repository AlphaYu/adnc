using System.Collections.Generic;
using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Dtos
{
    public class WarehouseBlockQtyDto : IDto
    {
        public long OrderId { get; set; }

        public ICollection<(long ProductId, int Qty)> Products { get; set; }
    }
}
