using Adnc.Infra.Repository.EfCore.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace Adnc.Infra.Repository.EfCore.Internal;

//internal class ExpressionHelper
public class ExpressionHelper
{
    public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> Transform<TEntity>(Expression<Func<TEntity, TEntity>> source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));

            // 创建一个参数表达式，类型为 SetPropertyCalls<TEntity>
            var parameter = Expression.Parameter(typeof(SetPropertyCalls<TEntity>), "x");

            // 获取源表达式的Body部分
            var body = source.Body;

            // 检查源表达式类型是否是 MemberInitExpression 类型
            if (body is MemberInitExpression memberInit)
            {
                // 初始化 SetPropertyCalls<TEntity> 的更新表达式
                Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls = setter => setter;

                // 收集表达式绑定
                foreach (var binding in memberInit.Bindings.OfType<MemberAssignment>())
                {
                    // 获取 SetPropertyCalls<TEntity> 中的 Entity 属性
                    var entityAccess = Expression.Property(parameter, "Entity");  // 获取 Entity 属性

                    // 使用 Expression.Property 来访问实体对象的属性
                    var property = Expression.Property(entityAccess, binding.Member.Name);

                    //// 通过 SetProperty 方法来设置实体的属性
                    //var setPropertyCall = Expression.Call(
                    //    setter,
                    //    typeof(SetPropertyCalls<TEntity>).GetMethod("SetProperty"),
                    //    Expression.Lambda(property, parameter),  // 将属性作为表达式传递
                    //    binding.Expression   // 对应的值
                    //);

                    //// 合并所有的 SetProperty 调用
                    //setPropertyCalls = setPropertyCalls.Append(setter => setPropertyCall);
                }

                return setPropertyCalls;
            }
        }
        throw new InvalidOperationException("Unsupported expression type");
    }
 }


