using Adnc.Core.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;

namespace Adnc.Core.Shared.IRepositories
{
    public static class RepositoryExtentions
    {
        public static Expression<Func<TEntity, object>>[] GetExpressions<TEntity>(this IEfRepository<TEntity> @this
            , params Expression<Func<TEntity, object>>[] expressions)
            where TEntity : EfEntity
        {
            return expressions;
        }
    }
}
