using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class WarehouseAllocateToProductDto : IDto
    {
        public string ProductId { get; set; }
    }
}