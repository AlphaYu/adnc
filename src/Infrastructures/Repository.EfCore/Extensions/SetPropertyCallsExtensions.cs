using Microsoft.EntityFrameworkCore.Query;

namespace Adnc.Infra.Repository.EfCore.Extensions;

public static class SetPropertyCallsExtensions
{
    public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> Append<TEntity>(this Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> left, Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> right)
    {
        var replace = new ReplacingExpressionVisitor(right.Parameters, [left.Body]);
        var combined = replace.Visit(right.Body);
        return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(combined, left.Parameters);
    }
}
