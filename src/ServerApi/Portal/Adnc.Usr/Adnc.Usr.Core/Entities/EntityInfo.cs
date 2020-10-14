using System;
using System.Collections.Concurrent;
using Adnc.Core.Shared.Entities;

namespace Adnc.Usr.Core.Entities
{
    public class EntityInfo : IEntityInfo
    {
        private static readonly ConcurrentBag<Type> bag = new ConcurrentBag<Type>()
        { 
             typeof(SysMenu),
             typeof(SysRelation),
             typeof(SysRole),
             typeof(SysUser),
             typeof(SysUserFinance)
        };

        public ConcurrentBag<Type> GetEntities()
        {
            return bag;
        }
    }
}