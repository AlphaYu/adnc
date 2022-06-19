namespace Microsoft.EntityFrameworkCore.Metadata
{
    /// <summary>
    /// https://www.cnblogs.com/willick/p/13358580.html
    /// 软删除过滤器
    /// </summary>
    public static class MutableEntityTypeExtension
    {
        public static void AddSoftDeleteQueryFilter(
            this IMutableEntityType entityData)
        {
            var methodToCall = typeof(MutableEntityTypeExtension)
                .GetMethod(nameof(GetSoftDeleteFilter),
                    BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);
            if (methodToCall == null) return;
            var filter = methodToCall.Invoke(null, Array.Empty<object>());
            if (filter is null)
                return;
            entityData.SetQueryFilter((LambdaExpression)filter);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, ISoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }
}