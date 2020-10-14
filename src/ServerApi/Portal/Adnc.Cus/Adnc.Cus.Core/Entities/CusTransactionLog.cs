using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Adnc.Core.Shared.Entities;

namespace Adnc.Cus.Core.Entities
{
    [Table("CusTransactionLog")]
    [Description("客户财务变动记录")]
    public class CusTransactionLog : EfEntity
    {
        public long CustomerId { get; set; }

        [StringLength(32)]
        public string Account { get; set; }

        [StringLength(3)]
        public string ExchangeType { get; set; }

        [StringLength(3)]
        public string ExchageStatus { get; set; }

        public decimal ChangingAmount { get; set; }

        public decimal Amount { get; set; }

        public decimal ChangedAmount { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

    }
}
