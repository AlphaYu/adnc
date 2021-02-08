using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class OpsLogDto : MongoDto
    {
        /// <summary>
        /// 控制器类名
        /// </summary>
        public string ClassName { get; set; }

		/// <summary>
		/// 日志业务名称
		/// </summary>
		public string LogName { get; set; }

		/// <summary>
		/// 日志类型
		/// </summary>
		public string LogType { get; set; }

		/// <summary>
		/// 详细信息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 控制器方法
		/// </summary>
		public string Method { get; set; }

		/// <summary>
		/// 是否操作成功
		/// </summary>
		public string Succeed { get; set; }

		/// <summary>
		/// 操作用户ID
		/// </summary>
		public long UserId { get; set; }

		/// <summary>
		/// 账号
		/// </summary>
		public string Account { get; set; }

		/// <summary>
		/// 操作用户名
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// Ip
		/// </summary>
		public string RemoteIpAddress { get; set; }

		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime? CreateTime { get; set; }
	}
}
