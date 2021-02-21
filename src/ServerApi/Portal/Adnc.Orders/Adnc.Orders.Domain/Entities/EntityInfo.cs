using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Orders.Domain.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        {
            typeof(Order)
            ,typeof(OrderItem)
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}