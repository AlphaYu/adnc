using System;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;

namespace Adnc.Infra.Common.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// A T extension method to determines whether the object is equal to any of the provided values.
        /// </summary>
        /// <param name="this">The object to be compared.</param>
        /// <param name="values">The value list to compare with the object.</param>
        /// <returns>true if the values list contains the object, else false.</returns>
        public static bool In([NotNull] this Enum @this, params Enum[] values)
        {
            return Array.IndexOf(values, @this) >= 0;
        }

        /// <summary>
        /// An object extension method that gets description attribute.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        /// <returns>The description attribute.</returns>
        public static string GetDescription([NotNull] this Enum value)
        {
            var attr = value.GetType().GetField(value.ToString())
                .GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description;
        }
    }
}
