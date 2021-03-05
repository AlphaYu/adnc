using Adnc.Application.Shared.Dtos;

namespace Adnc.Cus.Application.Dtos
{
    public class CustomerDto : OutputBaseAuditDto<string>
    {
        public string Account { get; set; }

        public string Nickname { get; set; }

        public string Realname { get; set; }

        public decimal FinanceInfoBalance { get; set; }
    }
}
