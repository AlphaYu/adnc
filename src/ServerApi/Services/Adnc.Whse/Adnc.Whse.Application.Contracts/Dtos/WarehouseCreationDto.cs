using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class WarehouseCreationDto : IDto
    {
        public string PositionCode { get; set; }

        public string PositionDescription { get; set; }
    }
}