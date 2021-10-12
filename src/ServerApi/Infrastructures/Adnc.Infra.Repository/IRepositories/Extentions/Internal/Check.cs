using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Adnc.Infra.Repository.IRepositories.Extentions.Internal
{
    [DebuggerStepThrough]
    internal static class Check
    {
        public static T NotNull<T>( T value, [NotNull] string parameterName)
        {
#pragma warning disable IDE0041 // Use 'is null' check
            if (ReferenceEquals(value, null))
#pragma warning restore IDE0041 // Use 'is null' check
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static IReadOnlyList<T> NotEmpty<T>(IReadOnlyList<T> value, [NotNull] string parameterName)
        {
            NotNull(value, parameterName);

            if (value.Count == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(AbstractionsStrings.CollectionArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static string NotEmpty(string value, [NotNull] string parameterName)
        {
            Exception e = null;
            if (value is null)
            {
                e = new ArgumentNullException(parameterName);
            }
            else if (value.Trim().Length == 0)
            {
                e = new ArgumentException(AbstractionsStrings.ArgumentIsEmpty(parameterName));
            }

            if (e != null)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw e;
            }

            return value;
        }

        public static string NullButNotEmpty(string value, [NotNull] string parameterName)
        {
            if (!(value is null)
                && value.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(AbstractionsStrings.ArgumentIsEmpty(parameterName));
            }

            return value;
        }

        public static IReadOnlyList<T> HasNoNulls<T>(IReadOnlyList<T> value, [NotNull] string parameterName)
            where T : class
        {
            NotNull(value, parameterName);

            if (value.Any(e => e == null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException(parameterName);
            }

            return value;
        }

        [Conditional("DEBUG")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General exceptions should never be thrown", Justification = "<挂起>")]
        public static void DebugAssert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"Check.DebugAssert failed: {message}");
            }
        }
    }
}
