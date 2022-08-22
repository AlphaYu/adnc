using Adnc.Shared.Entities;

namespace Adnc.Shared.Repository.Entities.Config;

public class EventTrackerConfig : EntityTypeConfiguration<EventTracker>
{
    public override void Configure(EntityTypeBuilder<EventTracker> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.TrackerName).HasMaxLength(50);
        builder.HasIndex(x => new { x.EventId, x.TrackerName }, "uk_eventid_trackername").IsUnique();
    }
}