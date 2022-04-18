using Adnc.Application.Shared.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProductPutOnSaleDto : IDto
    {
        public string Reason { get; set; }
    }
}