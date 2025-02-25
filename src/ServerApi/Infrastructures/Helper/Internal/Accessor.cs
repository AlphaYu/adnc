namespace Adnc.Infra.Helper.Internal;

/// <summary>
///   HttpContext Accessor
///   https://www.cnblogs.com/artech/p/how-to-get-httpcontext.html
/// </summary>
public sealed class Accessor
{
    private static Func<object>? _asyncLocalAccessor;
    private static Func<object, object>? _holderAccessor;
    private static Func<object, HttpContext>? _httpContextAccessor;

    internal Accessor()
    {
    }

    /// <summary>
    /// get current HttpContext
    /// </summary>
    /// <returns></returns>
    public HttpContext? GetCurrentHttpContext()
    {
        var asyncLocal = (_asyncLocalAccessor ??= CreateAsyncLocalAccessor())?.Invoke();
        if (asyncLocal is null)
        {
            return null;
        }

        var holder = (_holderAccessor ??= CreateHolderAccessor(asyncLocal))?.Invoke(asyncLocal);
        if (holder is null)
        {
            return null;
        }

        return (_httpContextAccessor ??= CreateHttpContextAccessor(holder))(holder);

        static Func<object>? CreateAsyncLocalAccessor()
        {
            var fieldInfo = typeof(HttpContextAccessor).GetField("_httpContextCurrent", BindingFlags.Static | BindingFlags.NonPublic);
            if (fieldInfo is null)
                return default;

            var field = Expression.Field(null, fieldInfo);
            return Expression.Lambda<Func<object>>(field).Compile();
        }

        static Func<object, object>? CreateHolderAccessor(object asyncLocal)
        {
            var holderType = asyncLocal.GetType().GetGenericArguments()[0];
            var method = typeof(AsyncLocal<>).MakeGenericType(holderType)?.GetProperty("Value")?.GetGetMethod();
            if (method is null)
                return default;
            var target = Expression.Parameter(typeof(object));
            var convert = Expression.Convert(target, asyncLocal.GetType());
            var getValue = Expression.Call(convert, method);
            return Expression.Lambda<Func<object, object>>(getValue, target).Compile();
        }

        static Func<object, HttpContext> CreateHttpContextAccessor(object holder)
        {
            var target = Expression.Parameter(typeof(object));
            var convert = Expression.Convert(target, holder.GetType());
            var field = Expression.Field(convert, "Context");
            var convertAsResult = Expression.Convert(field, typeof(HttpContext));
            return Expression.Lambda<Func<object, HttpContext>>(convertAsResult, target).Compile();
        }
    }
}