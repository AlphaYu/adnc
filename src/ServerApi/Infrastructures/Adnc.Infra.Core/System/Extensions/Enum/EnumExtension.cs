using System.ComponentModel;

namespace System
{
    public static class EnumExtension
    {
        /// <summary>
        /// An object extension method that gets description attribute.
        /// </summary>
        /// <param name="value">The value to act on.</param>
        /// <returns>The description attribute.</returns>
        public static string? GetDescription(this Enum value)
        {
            var attr = value?.GetType()
                                     ?.GetField(value.ToString())
                                     ?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description;
        }
    }
}