namespace System.Linq.Expressions
{
    public static class ExpressionLogicalOperatorsExtension
    {
        // https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool/457328#457328
        public static Expression<Func<T, bool>> Or<T>([NotNull] this Expression<Func<T, bool>> @this, Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(@this.Parameters[0], parameter);
            var left = leftVisitor.Visit(@this.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        public static Expression<Func<T, bool>> OrIf<T>([NotNull] this Expression<Func<T, bool>> @this, bool condition, Expression<Func<T, bool>> expr)
            => condition ? Or<T>(@this, expr) : @this;

        public static Expression<Func<T, bool>> And<T>([NotNull] this Expression<Func<T, bool>> @this, Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(@this.Parameters[0], parameter);
            var left = leftVisitor.Visit(@this.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> AndIf<T>([NotNull] this Expression<Func<T, bool>> @this, bool condition, Expression<Func<T, bool>> expr)
            => condition ? And<T>(@this, expr) : @this;

        [Obsolete("Obsoleted")]
        public static Expression<Func<T, bool>> True<T>()
            => f => true;

        [Obsolete("Obsoleted")]
        public static Expression<Func<T, bool>> False<T>()
            => f => false;

        private static MemberExpression ExtractMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return (MemberExpression)expression;
            }

            if (expression.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression).Operand;
                return ExtractMemberExpression(operand);
            }

            return null;
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;

                return base.Visit(node);
            }
        }
    }
}