using Adnc.Infra.Helper;
using Microsoft.Extensions.DependencyInjection;

namespace Adnc.Shared.Domain.Entities;

public abstract class AggregateRoot : DomainEntity, IConcurrency, IEfEntity<long>
{
    public byte[] RowVersion { get; set; }

    public Lazy<IEventPublisher> EventPublisher => new(() => InfraHelper.Accessor.GetCurrentHttpContext().RequestServices.GetRequiredService<IEventPublisher>());
}