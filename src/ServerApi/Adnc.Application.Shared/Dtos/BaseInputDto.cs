using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared.Dtos
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
        public virtual long Id { get; set; }
    }
}
