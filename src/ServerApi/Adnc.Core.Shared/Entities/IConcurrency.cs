using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface IConcurrency
    {
        public DateTime? RowVersion { get; set; }
    }
}
