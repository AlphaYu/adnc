﻿using Adnc.Infra.Entities.Config;
using Adnc.Shared.Consts.Entity.Cust;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adnc.Cus.Entities.Config
{
    public class CustomerTransactionLogConfig : EntityTypeConfiguration<CustomerTransactionLog>
    {
        public override void Configure(EntityTypeBuilder<CustomerTransactionLog> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Account).IsRequired().HasMaxLength(CustomerTransactionLogConsts.Account_MaxLength);
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.ChangingAmount).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.ChangedAmount).IsRequired().HasColumnType("decimal(18,4)");
            builder.Property(x => x.Remark).HasMaxLength(CustomerTransactionLogConsts.Remark_MaxLength);
        }
    }
}