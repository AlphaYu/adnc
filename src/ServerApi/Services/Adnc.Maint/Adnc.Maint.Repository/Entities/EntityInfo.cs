﻿namespace Adnc.Maint.Entities;

public class EntityInfo : AbstractEntityInfo
{
    public override (Assembly Assembly, IEnumerable<Type> Types) GetEntitiesInfo()
    {
        var assembly = this.GetType().Assembly;
        var entityTypes = base.GetEntityTypes(assembly);

        return (assembly, entityTypes);
    }
}