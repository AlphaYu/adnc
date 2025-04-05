namespace Adnc.Shared.Domain.Entities;

public abstract class AggregateRoot : DomainEntity, IConcurrency, IEfEntity<long>
{
    public Lazy<IEventPublisher> EventPublisher => new(() =>
    {
        return ServiceLocator.GetProvider().GetRequiredService<IEventPublisher>();
    });

    public byte[] RowVersion { get; set; } = [];
}
