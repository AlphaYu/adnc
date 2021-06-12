using Adnc.Infra.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adnc.Domain.Shared.Entities
{
    public abstract class AbstractDomainEntityInfo : AbstractEntityInfo
    {
        protected override IEnumerable<Type> GetEntityTypes(Assembly assembly)
        {
            var efEntities = assembly.GetTypes().Where(m =>
                                                       m.FullName != null
                                                       && (typeof(AggregateRoot).IsAssignableFrom(m) || typeof(DomainEntity).IsAssignableFrom(m))
                                                       && !m.IsAbstract).ToArray();

            return efEntities;
        }

        public override (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo()
        {
            return (default, default);
        }
    }
}