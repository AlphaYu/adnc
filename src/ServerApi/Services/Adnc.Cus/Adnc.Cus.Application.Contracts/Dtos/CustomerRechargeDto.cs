using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Contracts.Dtos
{
    public class CustomerRechargeDto : IInputDto
    {
        public decimal Amount { get; set; }
    }
}
