using Adnc.Core.Shared.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Cus.Core.Entities
{
    [Table("Customer")]
    [Description("客户表")]
    public class Customer : EfAuditEntity
    {
        [Required]
        [StringLength(32)]
        public string Account { get; set; }

        [Required]
        [StringLength(32)]
        public string Nickname { get; set; }

        [Required]
        [StringLength(20)]
        public string Realname { get; set; }

        public virtual CusFinance CusFinance { get; set; }
    }
}
