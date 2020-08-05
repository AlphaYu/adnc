using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Adnc.Core.Entities
{
    public class SysUserFinance: EfAuditEntity
    {

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("RowVersion")]
        [ConcurrencyCheck]
        public DateTime? RowVersion { get; set; }

        public virtual SysUser User { get; set; }
    }
}
