namespace Adnc.Demo.Admin.Repository.Entities.Config;

public class DictConfig : AbstractEntityTypeConfiguration<Dict>
{
    public override void Configure(EntityTypeBuilder<Dict> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Code).HasMaxLength(DictConsts.Code_MaxLength);
        builder.Property(x => x.Name).HasMaxLength(DictConsts.Name_MaxLength);
        builder.Property(x => x.Remark).HasMaxLength(DictConsts.Remark_MaxLength);
    }
}

public class DictDataConfig : AbstractEntityTypeConfiguration<DictData>
{
    public override void Configure(EntityTypeBuilder<DictData> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DictCode).HasMaxLength(DictConsts.Code_MaxLength);
        builder.Property(x => x.Label).HasMaxLength(DictDataConsts.Label_MaxLength);
        builder.Property(x => x.Value).HasMaxLength(DictDataConsts.Value_MaxLength);
        builder.Property(x => x.TagType).HasMaxLength(DictDataConsts.TagType_MaxLength);
    }
}
