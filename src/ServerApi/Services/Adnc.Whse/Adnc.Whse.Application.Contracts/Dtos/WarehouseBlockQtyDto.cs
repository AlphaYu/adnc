using Adnc.Infra.Application.Dtos;
using System.Collections.Generic;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class WarehouseBlockQtyDto : IDto
    {
        public long OrderId { get; set; }

        public ICollection<(long ProductId, int Qty)> Products { get; set; }
    }
}