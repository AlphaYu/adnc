using System.ComponentModel;
using Adnc.Core.Shared.Entities;

namespace Adnc.Cus.Core.Entities
{
    [Description("客户财务表")]
    public class CustomerFinance : EfFullAuditEntity, IConcurrency
    {
        public string Account { get; set; }

        public decimal Balance { get; set; }

        public virtual Customer Customer { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
