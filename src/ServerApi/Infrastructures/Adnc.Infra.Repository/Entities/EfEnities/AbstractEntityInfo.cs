namespace Adnc.Infra.Entities
{
    public abstract class AbstractEntityInfo : IEntityInfo
    {
        protected virtual IEnumerable<Type> GetEntityTypes(Assembly assembly)
        {
            var efEntities = assembly.GetTypes().Where(m =>
                                                       m.FullName != null
                                                       && typeof(EfEntity).IsAssignableFrom(m)
                                                       && !m.IsAbstract);

            return efEntities;
        }

        public abstract IEnumerable<EntityTypeInfo> GetEntitiesTypeInfo();
    }
}