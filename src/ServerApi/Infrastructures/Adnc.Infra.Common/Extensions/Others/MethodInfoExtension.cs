using System.Reflection;
using System.Threading.Tasks;

namespace Adnc.Infra.Common.Extensions
{
    public static class MethodInfoExtension
    {
        public static bool IsAsyncMethod(this MethodInfo @this)
        {
            return (
                @this.ReturnType == typeof(Task)
                || (@this.ReturnType.IsGenericType && @this.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            );
        }
    }
}