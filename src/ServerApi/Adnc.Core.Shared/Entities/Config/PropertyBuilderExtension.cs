using Adnc.Core.Shared.Entities.Consts;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    public static class PropertyBuilderExtension
    {
        public static PropertyBuilder<string> IsRequiredAndMaxLength(this PropertyBuilder<string> builder, EntityConst entConst)
        {
            builder.IsRequired(entConst.IsRequired);
            builder.HasMaxLength(entConst.MaxLength);
            return builder;
        }
    }
}
