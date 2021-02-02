using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Adnc.Core.Shared.EventBus
{
    public interface IEventHandler<TEto>
        where TEto : BaseEto
    {
        Task Process(TEto rechargeEbModel);
    }
}
