using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Warehouse.Core.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        {
             typeof(Product),
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}