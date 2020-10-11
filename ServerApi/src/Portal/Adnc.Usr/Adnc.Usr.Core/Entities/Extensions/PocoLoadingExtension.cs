using JetBrains.Annotations;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Adnc.Usr.Core.Entities
{
    public static class PocoLoadingExtension
    {
        public static TRelated Load<TRelated>(
             this Action<object, string> loader,
             object entity,
             ref TRelated navigationField,
             [CallerMemberName] string navigationName = null)
             where TRelated : class
        {
            loader?.Invoke(entity, navigationName);

            return navigationField;
        }

    }
}
