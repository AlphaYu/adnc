﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.Consul
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// 创建一个token
        /// </summary>
        /// <returns></returns>
        public string Create();
    }
}
