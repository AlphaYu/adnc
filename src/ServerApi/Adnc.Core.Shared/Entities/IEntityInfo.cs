using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adnc.Core.Shared.Entities
{
    public interface IEntityInfo
    {
        //[Obsolete("该接口已经废弃，请用GetEntitiesInfo()")]
        //ConcurrentBag<Type> GetEntities() { throw new Exception("该接口已经废弃"); }

        (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo();
    }
}
