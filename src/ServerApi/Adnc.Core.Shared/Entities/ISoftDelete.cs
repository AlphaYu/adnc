using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Core.Shared.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
