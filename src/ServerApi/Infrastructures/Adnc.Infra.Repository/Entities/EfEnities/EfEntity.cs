namespace Adnc.Infra.Entities
{
    public abstract class EfEntity : Entity, IEfEntity<long>
    {
    }
    public abstract class EfEntityNotKey : EntityNotKey, IEntityNotKey
    {

    }
}