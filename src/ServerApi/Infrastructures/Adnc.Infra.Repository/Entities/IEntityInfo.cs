using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adnc.Infra.Entities
{
    public interface IEntityInfo
    {
        (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo();
    }
}