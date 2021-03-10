using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Adnc.Core.Shared.Domain.Entities;

namespace Adnc.Core.Shared.Entities
{
    public abstract class AbstractEntityInfo : IEntityInfo
    {
        protected virtual IEnumerable<Type> GetEntityTypes(Assembly assembly)
        {
            var efEntities = assembly.GetTypes().Where(m =>
                                                       m.FullName != null
                                                       && typeof(EfEntity).IsAssignableFrom(m)
                                                       && !m.IsAbstract).ToArray();

            return efEntities;
        }

        protected virtual IEnumerable<Type> GetDDDObjectTypes(Assembly assembly)
        {
            var efEntities = assembly.GetTypes().Where(m =>
                                                       m.FullName != null
                                                       && (typeof(AggregateRoot).IsAssignableFrom(m) || typeof(Entity).IsAssignableFrom(m))
                                                       && !m.IsAbstract).ToArray();

            return efEntities;
        }

        public abstract (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo();
    }
}