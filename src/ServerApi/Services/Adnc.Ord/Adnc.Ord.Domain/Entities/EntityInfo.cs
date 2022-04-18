using Adnc.Domain.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adnc.Ord.Domain.Entities
{
    public class EntityInfo : AbstractDomainEntityInfo
    {
        public override (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo()
        {
            var assembly = this.GetType().Assembly;
            var entityTypes = base.GetEntityTypes(assembly);

            return (assembly, entityTypes);
        }
    }
}