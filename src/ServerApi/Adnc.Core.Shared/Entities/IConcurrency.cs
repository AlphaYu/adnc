using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface IConcurrency
    {
        /// <summary>
        /// 并发控制列
        /// </summary>
        public byte[] RowVersion { get; set; }
    }
}
