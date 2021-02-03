using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adnc.Warehouse.Core.Entities.Config
{
    public class ShelfConfig : IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Position.Code)
                   .HasColumnName("PositionCode")
                   .IsRequired()
                   .HasMaxLength(32);
            builder.Property(x => x.Position.Description)
                   .HasColumnName("PositionDescription")
                   .HasMaxLength(64);
        }
    }
}
