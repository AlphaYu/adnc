using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adnc.Core.Shared.Entities
{
    public abstract class EfEntity : IEfEntity<long>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public virtual long Id { get; set; }
    }
}
