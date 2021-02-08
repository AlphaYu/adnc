using Adnc.Application.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Usr.Application.Dtos
{
    public class UserChangeStatusDto : IDto
    {
        public long[] UserIds { get; set; }

        public int Status { get; set; }
    }
}
