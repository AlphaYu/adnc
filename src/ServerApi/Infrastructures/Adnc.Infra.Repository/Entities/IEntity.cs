namespace Adnc.Infra.Entities
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}