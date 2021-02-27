using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Whse.Domain.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        {
             typeof(Product),
             typeof(Shelf)
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}