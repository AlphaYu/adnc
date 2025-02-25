namespace System
{
    public static class DateTime2NdExtension
    {
        /// <summary>
        ///     A DateTime extension method that ages the given this.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>An int.</returns>
        public static int GetAge(this DateTime @this)
        {
            if (DateTime.Today.Month < @this.Month ||
                DateTime.Today.Month == @this.Month &&
                DateTime.Today.Day < @this.Day)
            {
                return DateTime.Today.Year - @this.Year - 1;
            }
            return DateTime.Today.Year - @this.Year;
        }

        /// <summary>
        ///     A DateTime extension method that query if '@this' is a week day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if '@this' is a week day, false if not.</returns>
        public static bool IsWeekDay(this DateTime @this)
        {
            return !(@this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday);
        }

        /// <summary>
        ///     A DateTime extension method that query if '@this' is a week day.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>true if '@this' is a week day, false if not.</returns>
        public static bool IsWeekendDay(this DateTime @this)
        {
            return @this.DayOfWeek == DayOfWeek.Saturday || @this.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        ///     A DateTime extension method that starts of week.
        /// </summary>
        /// <param name="dt">The dt to act on.</param>
        /// <param name="startDayOfWeek">(Optional) the start day of week.</param>
        /// <returns>A DateTime.</returns>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startDayOfWeek = DayOfWeek.Sunday)
        {
            var start = new DateTime(dt.Year, dt.Month, dt.Day);

            if (start.DayOfWeek != startDayOfWeek)
            {
                var d = startDayOfWeek - start.DayOfWeek;
                if (startDayOfWeek <= start.DayOfWeek)
                {
                    return start.AddDays(d);
                }
                return start.AddDays(-7 + d);
            }

            return start;
        }

        /// <summary>
        ///     A DateTime extension method that converts the @this to an epoch time span.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <returns>@this as a TimeSpan.</returns>
        public static TimeSpan ToEpochTimeSpan(this DateTime @this) => @this.ToUniversalTime().Subtract(new DateTime(1970, 1, 1));

        /// <summary>
        ///     A T extension method that check if the value is between inclusively the minValue and maxValue.
        /// </summary>
        /// <param name="this">The @this to act on.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <returns>true if the value is between inclusively the minValue and maxValue, otherwise false.</returns>
        public static bool InRange(this DateTime @this, DateTime minValue, DateTime maxValue)
        {
            return @this.CompareTo(minValue) >= 0 && @this.CompareTo(maxValue) <= 0;
        }

        /// <summary>
        ///     Converts a time to the time in a particular time zone.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>The date and time in the destination time zone.</returns>
        public static DateTime ConvertTime(this DateTime dateTime, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, destinationTimeZone);
        }

        /// <summary>
        ///     Converts a time from one time zone to another.
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of .</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>
        ///     The date and time in the destination time zone that corresponds to the  parameter in the source time zone.
        /// </returns>
        public static DateTime ConvertTime(this DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTime(dateTime, sourceTimeZone, destinationTimeZone);
        }

        /// <summary>
        ///     Converts a Coordinated Universal Time (UTC) to the time in a specified time zone.
        /// </summary>
        /// <param name="dateTime">The Coordinated Universal Time (UTC).</param>
        /// <param name="destinationTimeZone">The time zone to convert  to.</param>
        /// <returns>
        ///     The date and time in the destination time zone. Its  property is  if  is ; otherwise, its  property is .
        /// </returns>
        public static DateTime ConvertTimeFromUtc(this DateTime dateTime, TimeZoneInfo destinationTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, destinationTimeZone);
        }

        /// <summary>
        ///     Converts the current date and time to Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <returns>
        ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  value&#39;s  property is always
        ///     set to .
        /// </returns>
        public static DateTime ConvertTimeToUtc(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime);
        }

        /// <summary>
        ///     Converts the time in a specified time zone to Coordinated Universal Time (UTC).
        /// </summary>
        /// <param name="dateTime">The date and time to convert.</param>
        /// <param name="sourceTimeZone">The time zone of .</param>
        /// <returns>
        ///     The Coordinated Universal Time (UTC) that corresponds to the  parameter. The  object&#39;s  property is
        ///     always set to .
        /// </returns>
        public static DateTime ConvertTimeToUtc(this DateTime dateTime, TimeZoneInfo sourceTimeZone)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
        }

        /// <summary>
        /// ToDateString("yyyy-MM-dd")
        /// </summary>
        /// <param name="this">dateTime</param>
        /// <returns></returns>
        public static string ToStandardDateString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// ToTimeString("yyyy-MM-dd HH:mm:ss")
        /// </summary>
        /// <param name="this">datetime</param>
        /// <returns></returns>
        public static string ToStandardTimeString(this DateTime @this)
        {
            return @this.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}