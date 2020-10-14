using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace  Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class LoginLogSaveInputDto : BaseInputDto
    {
        public string IP { get; set; }

        public string Device { get; set; }

        public string Message { get; set; }

        public bool Succeed { get; set; }

        public int? UserId { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string RemoteIpAddress { get; set; }

        public DateTime? CreateTime { get; set; }

    }
}
