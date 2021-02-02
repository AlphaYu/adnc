using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Maint.Core.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        {
             typeof(SysCfg),
             typeof(SysDict),
             typeof(SysNotice),
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}