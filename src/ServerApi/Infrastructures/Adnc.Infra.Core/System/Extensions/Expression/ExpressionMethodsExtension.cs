namespace System.Linq.Expressions
{
    public static class ExpressionMethodsExtension
    {
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

        public static MethodCallExpression GetMethodExpression<T>(this Expression<Action<T>> method)
        {
            if (method.Body.NodeType != ExpressionType.Call)
                throw new ArgumentException("Method call expected", method.Body.ToString());
            return (MethodCallExpression)method.Body;
        }

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
        /// GetMemberName
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <typeparam name="TMember">TMember</typeparam>
        /// <param name="memberExpression">get member expression</param>
        /// <returns></returns>
        public static string? GetMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> memberExpression)=>
            GetMemberInfo(memberExpression)?.Name;

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
    }
}