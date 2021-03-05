using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Cus.Core.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        {
             typeof(Customer),
             typeof(CustomerFinance),
             typeof(CustomerTransactionLog)
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}