namespace System.Linq.Expressions
{
    /// <summary>
    /// Extension methods for combining expressions using logical operators.
    /// https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool/457328#457328
    /// </summary>
    public static class ExpressionLogicalOperatorsExtension
    {
        /// <summary>
        /// Combine two expressions using the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> firstExpr, Expression<Func<T, bool>> secondExpr)
            where T : class
        {
            var parameter = Expression.Parameter(typeof(T));
            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(secondExpr.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpr.Body);
            if (left is null || right is null)
                return firstExpr;
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        /// <summary>
        /// Combine two expressions using the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T1, T2, bool>> Or<T1, T2>(this Expression<Func<T1, T2, bool>> firstExpr, Expression<Func<T1, T2, bool>> secondExpr)
            where T1 : class
            where T2 : class
        {
            var parameter = Expression.Parameter(typeof(T1));
            var parameter2 = Expression.Parameter(typeof(T2));
            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(secondExpr.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpr.Body);
            if (left is null || right is null)
                return firstExpr;
            return Expression.Lambda<Func<T1, T2, bool>>(
                Expression.OrElse(left, right), parameter, parameter2);
        }

        /// <summary>
        /// Combine two expressions using the logical AND operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> firstExpr, Expression<Func<T, bool>> secondExpr)
            where T : class
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(secondExpr.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpr.Body);

            if (left is null || right is null)
                return firstExpr;

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }

        /// <summary>
        /// Combine two expressions using the logical AND operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T1, T2, bool>> And<T1, T2>(this Expression<Func<T1, T2, bool>> firstExpr, Expression<Func<T1, T2, bool>> secondExpr)
            where T1 : class
            where T2 : class
        {
            var parameter = Expression.Parameter(typeof(T1));
            var parameter2 = Expression.Parameter(typeof(T2));
            var leftVisitor = new ReplaceExpressionVisitor(firstExpr.Parameters[0], parameter);
            var left = leftVisitor.Visit(firstExpr.Body);
            var rightVisitor = new ReplaceExpressionVisitor(secondExpr.Parameters[0], parameter);
            var right = rightVisitor.Visit(secondExpr.Body);
            if (left is null || right is null)
                return firstExpr;
            return Expression.Lambda<Func<T1, T2, bool>>(
                Expression.AndAlso(left, right), parameter, parameter2);
        }

        /// <summary>
        /// Combine two expressions using the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> firstExpr, bool condition, Expression<Func<T, bool>> secondExpr)
            where T : class
        {
            return condition ? Or(firstExpr, secondExpr) : firstExpr;
        }

        /// <summary>
        /// Combine two expressions using the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T1, T2, bool>> OrIf<T1, T2>(this Expression<Func<T1, T2, bool>> firstExpr, bool condition, Expression<Func<T1, T2, bool>> secondExpr)
        where T1 : class
        where T2 : class
        {
            return condition ? Or(firstExpr, secondExpr) : firstExpr;
        }
        /// <summary>
        /// Combine two expressions using the logical AND operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> firstExpr, bool condition, Expression<Func<T, bool>> secondExpr)
            where T : class
        {
            return condition ? And(firstExpr, secondExpr) : firstExpr;
        }

        /// <summary>
        /// Combine two expressions using the logical AND operator.
        /// </summary>
        /// <typeparam name="T">The type of the object being tested.</typeparam>
        /// <param name="firstExpr">The first expression.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="secondExpr">The second expression.</param>
        /// <returns>The combined expression.</returns>
        public static Expression<Func<T1, T2, bool>> AndIf<T1, T2>(this Expression<Func<T1, T2, bool>> firstExpr, bool condition, Expression<Func<T1, T2, bool>> secondExpr)
            where T1 : class
            where T2 : class
        {
            return condition ? And(firstExpr, secondExpr) : firstExpr;
        }

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