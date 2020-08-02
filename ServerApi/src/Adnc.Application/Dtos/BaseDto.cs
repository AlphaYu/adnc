using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Adnc.Application.Dtos
{
	/// <summary>
	/// DTO基类
	/// </summary>
	/// <typeparam name="T"></typeparam>
    [Serializable]
    public class BaseDto<T>
    {
		/// <summary>
		/// 主键ID
		/// </summary>
		public T ID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Description("创建人")]
        public long? CreateBy { get; set; }

        /// <summary>
        /// 创建时间/注册时间
        /// </summary>
        [Description("创建时间/注册时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        [Description("最后更新人")]
        public long? ModifyBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Description("最后更新时间")]
        public DateTime? ModifyTime { get; set; }
    }
}
