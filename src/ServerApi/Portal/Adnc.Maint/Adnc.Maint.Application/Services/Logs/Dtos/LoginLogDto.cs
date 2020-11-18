using System;
using System.Collections.Generic;
using System.Text;
using Adnc.Application.Shared.Dtos;
using Adnc.Maint.Core.Entities;

namespace  Adnc.Maint.Application.Dtos
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLogDto : BaseInputDto
    {
        public string IP { get; set; }

        public string Device { get; set; }

        public string Message { get; set; }

        public bool Succeed { get; set; }

        public long? UserId { get; set; }

        public string Account { get; set; }

        public string UserName { get; set; }

        public string RemoteIpAddress { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
