namespace Adnc.Shared.Repository.EfCoreEntities.Config;

public class EventTrackerConfig : IEntityTypeConfiguration<EventTracker>
{
    public void Configure(EntityTypeBuilder<EventTracker> builder)
    {
        builder.Property(x => x.TrackerName).HasMaxLength(50);
        builder.HasIndex(x => new { x.EventId, x.TrackerName }, "uk_eventid_trackername").IsUnique();
    }
}
