using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Adnc.Core.Shared.Entities;

namespace Adnc.Cus.Core.Entities
{
    [Table("CusFinance")]
    [Description("客户财务表")]
    public class CusFinance : EfFullAuditEntity
    {
        [Required]
        [StringLength(16)]
        public string Account { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
