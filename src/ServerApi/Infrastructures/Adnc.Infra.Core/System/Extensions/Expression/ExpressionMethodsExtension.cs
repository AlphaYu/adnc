namespace System.Linq.Expressions
{
    /// <summary>
    /// Provides extension methods for working with System.Linq.Expressions.
    /// </summary>
    public static class ExpressionMethodsExtension
    {
        /// <summary>
        /// Gets the <see cref="MethodInfo"/> from an <see cref="Expression{TDelegate}"/> representing a method call.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="expression">The expression to extract the method information from.</param>
        /// <returns>The <see cref="MethodInfo"/> representing the method call.</returns>
        public static MethodInfo GetMethod<T>(this Expression<T> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MethodCallExpression methodCallExpression))
            {
                throw new InvalidCastException("Cannot be converted to MethodCallExpression");
            }
            return methodCallExpression.Method;
        }

        /// <summary>
        /// Extracts the <see cref="MethodCallExpression"/> from an <see cref="Expression{TDelegate}"/> representing a method call.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="method">The expression to extract the <see cref="MethodCallExpression"/> from.</param>
        /// <returns>The <see cref="MethodCallExpression"/> representing the method call.</returns>
        public static MethodCallExpression GetMethodExpression<T>(this Expression<Action<T>> method)
        {
            if (method.Body.NodeType != ExpressionType.Call)
                throw new ArgumentException("Method call expected", method.Body.ToString());
            return (MethodCallExpression)method.Body;
        }

        /// <summary>
        /// Extracts the <see cref="MethodCallExpression"/> from an <see cref="Expression{TDelegate}"/> representing a method call or a conversion.
        /// </summary>
        /// <typeparam name="T">The type of the delegate.</typeparam>
        /// <param name="exp">The expression to extract the <see cref="MethodCallExpression"/> from.</param>
        /// <returns>The <see cref="MethodCallExpression"/> representing the method call.</returns>
        public static MethodCallExpression GetMethodExpression<T>(this Expression<Func<T, object>> exp)
        {
            switch (exp.Body.NodeType)
            {
                case ExpressionType.Call:
                    return (MethodCallExpression)exp.Body;

                case ExpressionType.Convert:
                    if (exp.Body is UnaryExpression unaryExp && unaryExp.Operand is MethodCallExpression methodCallExpression)
                    {
                        return methodCallExpression;
                    }
                    throw new InvalidOperationException($"Method expected: {exp.Body}");

                default:
                    throw new InvalidOperationException("Method expected:" + exp.Body.ToString());
            }
        }

        /// <summary>
        /// Extracts the name of a member from an <see cref="Expression{TDelegate}"/> representing a member access.
        /// </summary>
        /// <typeparam name="TEntity">The type of the object containing the member.</typeparam>
        /// <typeparam name="TMember">The type of the member being accessed.</typeparam>
        /// <param name="memberExpression">The expression representing the member access.</param>
        /// <returns>The name of the member.</returns>
        public static string? GetMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> memberExpression) =>
            GetMemberInfo(memberExpression)?.Name;

        /// <summary>
        /// Extracts the <see cref="MemberInfo"/> from an <see cref="Expression{TDelegate}"/> representing a member access.
        /// </summary>
        /// <typeparam name="TEntity">The type of the object containing
        /// <summary>
        /// GetMemberInfo
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <typeparam name="TMember">TMember</typeparam>
        /// <param name="expression">get member expression</param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo<TEntity, TMember>([NotNull] this Expression<Func<TEntity, TMember>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new ArgumentException(nameof(expression));

            var lambda = (LambdaExpression)expression;

            var memberExpression = ExtractMemberExpression(lambda.Body);
            if (memberExpression == null)
                throw new ArgumentException(nameof(expression));

            return memberExpression.Member;
        }

        /// <summary>
        /// GetPropertyInfo
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo? GetProperty<TEntity, TProperty>([NotNull] this Expression<Func<TEntity, TProperty>> expression)
        {
            var member = GetMemberInfo(expression);
            if (null == member)
                throw new InvalidOperationException("no property found");

            if (member is PropertyInfo property)
                return property;

            return typeof(TEntity).GetProperty(member.Name);
        }

        private static MemberExpression? ExtractMemberExpression(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (MemberExpression)expression;

                case ExpressionType.Convert:
                    var operand = ((UnaryExpression)expression).Operand;
                    return ExtractMemberExpression(operand);

                default:
                    return null;
            }
        }
    }
}
