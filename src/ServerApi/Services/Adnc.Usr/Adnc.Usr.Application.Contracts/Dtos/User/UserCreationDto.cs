using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Contracts.Dtos
{
    public class UserCreationDto : UserCreationAndUpdationDto
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
