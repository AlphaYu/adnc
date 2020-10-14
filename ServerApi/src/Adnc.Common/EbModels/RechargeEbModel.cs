using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Common.EbModels
{
    public class RechargeEbModel
    {
        public long ID { get; set; }

        public decimal Amount { get; set; }

        public long TransactionLogId { get; set; }
    }
}
