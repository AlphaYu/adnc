using System.Collections.Generic;
using System.ComponentModel;
using Adnc.Core.Shared.Entities;

namespace Adnc.Cus.Core.Entities
{
    [Description("客户表")]
    public class Customer : EfFullAuditEntity
    {
        public string Account { get; set; }

        public string Nickname { get; set; }

        public string Realname { get; set; }

        public virtual CustomerFinance FinanceInfo { get; set; }

        public virtual ICollection<CustomerTransactionLog> TransactionLogs { get; set; }
    }
}
