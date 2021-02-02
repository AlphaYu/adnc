using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Adnc.Core.Shared.Entities;

namespace Adnc.Core.Shared.Domain.Entities
{
    public abstract class Entity : IEntity<long>
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [Key]
        public long Id { get; set; }
    }
}
