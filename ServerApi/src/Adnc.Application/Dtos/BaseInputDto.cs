using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 输入DTO基类
    /// </summary>
    [Serializable]
    public abstract class BaseInputDto : BaseDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public virtual long ID { get; set; }
    }
}
