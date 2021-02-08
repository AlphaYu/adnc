using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class UserValidateDto : OutputDto<long>
    {
        public string Account { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string RoleIds { get; set; }
    }
}
