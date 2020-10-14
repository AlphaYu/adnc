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
             typeof(CusFinance),
             typeof(CusTransactionLog)
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}