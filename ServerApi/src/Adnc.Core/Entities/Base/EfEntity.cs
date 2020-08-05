using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Adnc.Core.Entities
{
    public abstract class EfEntity : IEfEntity<long>
    {
        [Key]
        [Column("ID")]
        public virtual long ID { get; set; }
    }
}
