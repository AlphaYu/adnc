using Adnc.Infra.Entities;
using System;
using System.Linq.Expressions;

namespace Adnc.Infra.IRepository
{
    public static class RepositoryExtentions
    {
        public static Expression<Func<TEntity, object>>[] GetExpressions<TEntity>(this IEfRepository<TEntity> repository
            , params Expression<Func<TEntity, object>>[] expressions)
            where TEntity : EfEntity
        {
            return expressions;
        }
    }
}