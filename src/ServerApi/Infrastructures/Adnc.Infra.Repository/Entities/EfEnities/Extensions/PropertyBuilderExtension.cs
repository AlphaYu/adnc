using Adnc.Infra.Entities.Config;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    public static class PropertyBuilderExtension
    {
        public static PropertyBuilder<string> IsRequiredAndMaxLength(this PropertyBuilder<string> builder, EntityProperty entConst)
        {
            builder.IsRequired(entConst.IsRequired);
            builder.HasMaxLength(entConst.MaxLength);
            return builder;
        }
    }
}