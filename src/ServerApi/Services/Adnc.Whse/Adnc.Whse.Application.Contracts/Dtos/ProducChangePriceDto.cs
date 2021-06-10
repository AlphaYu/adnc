using Adnc.Infra.Application.Dtos;

namespace Adnc.Whse.Application.Contracts.Dtos
{
    public class ProducChangePriceDto : IDto
    {
        public decimal Price { set; get; }
    }
}