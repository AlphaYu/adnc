using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EfEntity : IEfEntity<long>
    {
        [Key]
        [Column("ID")]
        public virtual long ID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        [Column("CreateBy")]
        public virtual long? CreateBy { get; set; }

        /// <summary>
        /// 创建时间/注册时间
        /// </summary>
        [Description("创建时间/注册时间")]
        [Column("CreateTime")]
        public virtual DateTime? CreateTime { get; set; }
    }
}
