using System.Reflection;
using Microsoft.EntityFrameworkCore.Query;

namespace Adnc.Infra.Repository.EfCore.Internal;

//internal class ExpressionHelper
public static class ExpressionHelper
{
    public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> ConvertToSetPropertyCalls<TEntity>(Expression<Func<TEntity, TEntity>> expression)
    {
        var parameterName = "a";
        var param = Expression.Parameter(typeof(SetPropertyCalls<TEntity>), parameterName);
        Expression? constructorExpressions = null;
        if (expression.Body is MemberInitExpression memberInitExpression)
        {
            foreach (var item in memberInitExpression.Bindings)
            {
                if (item is MemberAssignment assignment)
                {
                    var propertyInfo = (PropertyInfo)assignment.Member;
                    var propertyName = assignment.Member.Name;
                    var properrtyType = propertyInfo.PropertyType;

                    var parameter = Expression.Parameter(typeof(TEntity), parameterName);
                    var propertyAccess = Expression.Property(parameter, propertyName);
                    var propertyExpression = Expression.Lambda(propertyAccess, parameter);

                    var valueExpression = assignment.Expression;

                    var valueParameter = Expression.Parameter(typeof(TEntity), parameterName);
                    var valueExpressionFunc = Expression.Lambda(valueExpression, valueParameter);

                    var setPropertyMethod = typeof(SetPropertyCalls<>)
                            .MakeGenericType(typeof(TEntity))
                            .GetMethods()
                            .Where(m => m.Name == "SetProperty"
                            && m.GetParameters()
                                .Where(p =>
                                p.ParameterType.IsGenericType &&
                                p.ParameterType.GetGenericTypeDefinition() == typeof(Func<,>)).Count() == 2)
                             .First();

                    setPropertyMethod = setPropertyMethod.MakeGenericMethod(properrtyType);
                    var setPropertyCall = Expression.Call(
                        constructorExpressions ?? param,
                        setPropertyMethod,
                        propertyExpression,
                        valueExpressionFunc
                    );
                    constructorExpressions = setPropertyCall;
                }
            }
        }
        return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(constructorExpressions ?? param, param);
    }
}

