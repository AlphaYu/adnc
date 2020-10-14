using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.RpcServices
{
    public class LoginReply
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
