using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Core.Shared.Events;

namespace Adnc.Cus.Core.EventBus.Etos
{
    public class CustomerRechargedEto : BaseEto
    {
        public decimal Amount { get; set; }

        public long TransactionLogId { get; set; }
    }
}
