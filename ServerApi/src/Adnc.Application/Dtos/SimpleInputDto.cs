using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Application.Dtos
{
    /// <summary>
    /// 用于解决API frompost 方式接收 string,int,long等基础类型的问题。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SimpleInputDto<T>
    {
        /// <summary>
        /// 需要传递的值
        /// </summary>
        public T Value { get; set; }
    }
}
