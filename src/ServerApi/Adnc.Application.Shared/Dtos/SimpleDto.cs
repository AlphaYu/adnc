using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Shared.Dtos
{
    /// <summary>
    /// 用于解决返回简单类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class SimpleDto<T> : BaseDto
    {
        public SimpleDto() { }

        public SimpleDto(T value)
        {
            Value = value;
        }

        /// <summary>
        /// 需要传递的值
        /// </summary>
        public T Value { get; set; }
    }
}
