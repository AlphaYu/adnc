namespace System.Linq.Expressions
{
    public static class ExpressionLogicalOperatorsExtension
    {
        // https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool/457328#457328
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> firstExpr, Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr.Body);

            if(left is null || right is null)
                return firstExpr;

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> firstExpr, bool condition, Expression<Func<T, bool>> expr)=>
           condition ? Or<T>(firstExpr, expr) : firstExpr;

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> firstExpr, Expression<Func<T, bool>> expr)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr.Body);

            if (left is null || right is null)
                return firstExpr;

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        public static Expression<Func<T, bool>> AndIf<T>([NotNull] this Expression<Func<T, bool>> firstExpr, bool condition, Expression<Func<T, bool>> expr)=>
            condition ? And<T>(firstExpr, expr) : firstExpr;

        private static MemberExpression? ExtractMemberExpression(Expression expression)
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

            return default;
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

            public override Expression? Visit(Expression? node)
            {
                if (node == _oldValue)
                    return _newValue;

                return base.Visit(node);
            }
        }
    }
}