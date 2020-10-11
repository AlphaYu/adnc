using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Adnc.Core.Shared.Entities
{
    public interface IEntityInfo
    {
        ConcurrentBag<Type> GetEntities();
    }
}
