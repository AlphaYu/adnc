using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Infr.EfCore
{
    public class UnitOfWorkStatus
    {
        public bool IsStartingUow { get; internal set; }
    }
}
