using JetBrains.Annotations;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Adnc.Infra.Common.Extensions
{
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> True<T>()
            {
                return f => true;
            }

        public static Expression<Func<T, bool>> False<T>()
            {
                return f => false;
            }

        //public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        //    {
        //        var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
        //        return Expression.Lambda<Func<T, bool>>(Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);
        //    }

        //public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        //    {
        //        var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());
        //        return Expression.Lambda<Func<T, bool>>(Expression.And(expression1.Body, invokedExpression), expression1.Parameters);
        //    }


        // https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool/457328#457328
        public static Expression<Func<T, bool>> Or<T>([NotNull] this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(left, right), parameter);
        }

        public static Expression<Func<T, bool>> And<T>([NotNull] this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
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
        public static string GetMemberName<TEntity, TMember>([NotNull] this Expression<Func<TEntity, TMember>> memberExpression) =>
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
            {
                throw new ArgumentException(nameof(expression));
                //throw new ArgumentException(string.Format(Resource.propertyExpression_must_be_lambda_expression, nameof(expression)), nameof(expression));
            }

            var lambda = (LambdaExpression)expression;

            var memberExpression = ExtractMemberExpression(lambda.Body);
            if (memberExpression == null)
            {
                throw new ArgumentException(nameof(expression));
                //throw new ArgumentException(string.Format(Resource.propertyExpression_must_be_lambda_expression, nameof(memberExpression)), nameof(memberExpression));
            }
            return memberExpression.Member;
        }

        /// <summary>
        /// GetPropertyInfo
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static PropertyInfo GetProperty<TEntity, TProperty>(
            [NotNull] this Expression<Func<TEntity, TProperty>> expression)
        {
            var member = GetMemberInfo(expression);
            if (null == member)
                throw new InvalidOperationException("no property found");

            if (member is PropertyInfo property)
                return property;

            return typeof(TEntity).GetProperty(member.Name);
        }

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
    }

}
