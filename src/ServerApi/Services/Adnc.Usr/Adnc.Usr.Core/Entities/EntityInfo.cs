using Adnc.Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adnc.Usr.Core.Entities
{
    public class EntityInfo : AbstractEntityInfo
    {
        public override (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo()
        {
            var assembly = this.GetType().Assembly;
            var entityTypes = base.GetEntityTypes(assembly);

            return (assembly, entityTypes);
        }
    }
}