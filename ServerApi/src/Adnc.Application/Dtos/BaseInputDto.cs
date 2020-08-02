using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 输入DTO基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseInputDto<T>
    {  
        /// <summary>
        /// 主键ID
        /// </summary>
        public T ID { get; set; }
    }
}
