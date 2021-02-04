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

            builder.Property(x => x.Id)
                   .ValueGeneratedNever();

            builder.OwnsOne(x=>x.Position)
                   .Property(x => x.Code)
                   .IsRequired()
                   .HasColumnName("PositionCode")
                   .HasMaxLength(32);

            builder.OwnsOne(x=>x.Position)
                   .Property(x => x.Description)
                   .HasColumnName("PositionDescription")
                   .HasMaxLength(64);
        }
    }
}
