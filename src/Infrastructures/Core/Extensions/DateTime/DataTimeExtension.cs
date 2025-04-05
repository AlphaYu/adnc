using System.Diagnostics;
using System.Globalization;

namespace System;

/// <summary>
/// Extension methods for DateTime.
/// </summary>
public static class DataTimeExtension
{
    /// <summary>
    /// timestamp=>datetime
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ToLocalTime(this long timestamp)
    {
        var dto = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        return dto.ToLocalTime().DateTime;
    }

    /// <summary>
    /// Gets the number of weeks in a given year.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="year">The year</param>
    /// <returns>The number of weeks in that year</returns>
    public static int GetWeekAmount(this DateTime _, int year)
    {
        var end = new DateTime(year, 12, 31); //该年最后一天
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday); //该年星期数
    }

    /// <summary>
    /// Returns the week number of the year. By default, Sunday is considered the first day of the week.
    /// </summary>
    /// <param name="value">The date</param>
    /// <returns>The week number</returns>
    public static int WeekOfYear(this in DateTime value)
    {
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
    }

    /// <summary>
    /// Returns the week number of the year.
    /// </summary>
    /// <param name="date">The date</param>
    /// <param name="week">The starting day of the week</param>
    /// <returns>The week number</returns>
    public static int WeekOfYear(this in DateTime date, DayOfWeek week)
    {
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, week);
    }

    /// <summary>
    /// Gets the start and end dates of a specific week in a given year.
    /// Year nYear
    /// Week number nNumWeek
    /// Week start out dtWeekStart
    /// Week end out dtWeekEnd
    /// </summary>
    /// <param name="_"></param>
    /// <param name="nYear">The year</param>
    /// <param name="nNumWeek">The week number</param>
    /// <param name="dtWeekStart">The start date of the week</param>
    /// <param name="dtWeekEnd">The end date of the week</param>
    public static void GetWeekTime(this DateTime _, int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekEnd)
    {
        var dt = new DateTime(nYear, 1, 1);
        dt += new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
        dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
        dtWeekEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
    }

    /// <summary>
    /// Gets the start and end dates of a specific week in a given year, from Monday to Friday (weekdays).
    /// </summary>
    /// <param name="_"></param>
    /// <param name="nYear">The year</param>
    /// <param name="nNumWeek">The week number</param>
    /// <param name="dtWeekStart">The start date of the week</param>
    /// <param name="dtWeekEnd">The end date of the week</param>
    public static void GetWeekWorkTime(this DateTime _, int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekEnd)
    {
        var dt = new DateTime(nYear, 1, 1);
        dt += new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
        dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
        dtWeekEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1).AddDays(-2);
    }

    /// <summary>
    /// Returns the relative number of days compared to the current date.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="relativeday">The relative number of days</param>
    public static string GetDateTime(this in DateTime value, int relativeday)
    {
        return value.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Gets the number of seconds that have passed since 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalSeconds(this in DateTime value) => new DateTimeOffset(value).ToUnixTimeSeconds();

    /// <summary>
    /// Gets the number of milliseconds that have passed since 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalMilliseconds(this in DateTime value) => new DateTimeOffset(value).ToUnixTimeMilliseconds();

    /// <summary>
    /// Gets the microsecond timestamp relative to 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long GetTotalMicroseconds(this in DateTime value) => new DateTimeOffset(value).Ticks / 10;

    /// <summary>
    /// Gets the nanosecond timestamp relative to 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long GetTotalNanoseconds(this in DateTime value) => new DateTimeOffset(value).Ticks * 100 + Stopwatch.GetTimestamp() % 100;

    /// <summary>
    /// Gets the number of minutes that have passed since 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalMinutes(this in DateTime value) => new DateTimeOffset(value).Offset.TotalMinutes;

    /// <summary>
    /// Gets the number of hours that have passed since 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalHours(this in DateTime value) => new DateTimeOffset(value).Offset.TotalHours;

    /// <summary>
    /// Gets the number of days that have passed since 1970-01-01 00:00:00 (Unix epoch).
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalDays(this in DateTime value) => new DateTimeOffset(value).Offset.TotalDays;

    /// <summary>
    /// Returns the number of days in the current year.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="iYear">The year</param>
    /// <returns>The number of days in the year</returns>
    public static int GetDaysOfYear(this DateTime _, int iYear)
    {
        return IsRuYear(iYear) ? 366 : 365;
    }

    /// <summary> 
    /// Returns the number of days in the current year. 
    /// </summary>
    /// <param name="value">The date</param>
    /// <returns>The day of the year for the given date</returns>
    public static int GetDaysOfYear(this in DateTime value)
    {
        //取得传入参数的年份部分，用来判断是否是闰年
        var n = value.Year;
        return IsRuYear(n) ? 366 : 365;
    }

    /// <summary> 
    /// Returns the number of days in the current month. 
    /// </summary>
    /// <param name="_"></param>
    /// <param name="iYear">The year</param>
    /// <param name="month">The month</param>
    /// <returns>The number of days in the month</returns>
    public static int GetDaysOfMonth(this DateTime _, int iYear, int month)
    {
        return month switch
        {
            1 => 31,
            2 => (IsRuYear(iYear) ? 29 : 28),
            3 => 31,
            4 => 30,
            5 => 31,
            6 => 30,
            7 => 31,
            8 => 31,
            9 => 30,
            10 => 31,
            11 => 30,
            12 => 31,
            _ => 0
        };
    }

    /// <summary> 
    /// Returns the number of days in the current month. 
    /// </summary>
    /// <param name="value">The date</param>
    /// <returns>The number of days in the month</returns>
    public static int GetDaysOfMonth(this in DateTime value)
    {
        // Uses the year and month information to get the number of days in the current month.
        return value.Month switch
        {
            1 => 31,
            2 => (IsRuYear(value.Year) ? 29 : 28),
            3 => 31,
            4 => 30,
            5 => 31,
            6 => 30,
            7 => 31,
            8 => 31,
            9 => 30,
            10 => 31,
            11 => 30,
            12 => 31,
            _ => 0
        };
    }

    /// <summary>
    /// Returns the name of the weekday for the given date.
    /// </summary>
    /// <param name="value">The date</param>
    /// <returns>The name of the weekday</returns>
    public static string GetWeekNameOfDay(this in DateTime value)
    {
        return value.DayOfWeek.ToString() switch
        {
            "Mondy" => "星期一",
            "Tuesday" => "星期二",
            "Wednesday" => "星期三",
            "Thursday" => "星期四",
            "Friday" => "星期五",
            "Saturday" => "星期六",
            "Sunday" => "星期日",
            _ => ""
        };
    }

    /// <summary>
    /// Returns the weekday number for the given date.
    /// </summary>
    /// <param name="value">The date</param>
    /// <returns>The weekday number</returns>
    public static string GetWeekNumberOfDay(this in DateTime value)
    {
        return value.DayOfWeek.ToString() switch
        {
            "Mondy" => "1",
            "Tuesday" => "2",
            "Wednesday" => "3",
            "Thursday" => "4",
            "Friday" => "5",
            "Saturday" => "6",
            "Sunday" => "7",
            _ => ""
        };
    }

    /// <summary>
    /// Determines whether the given year is a leap year. Private function.
    /// </summary>
    /// <param name="value">The year</param>
    /// <returns>True if it's a leap year, False if it's not a leap year</returns>
    private static bool IsRuYear(int value)
    {
        // The parameter is the year.
        // Example: 2003
        var n = value;
        return n % 400 == 0 || n % 4 == 0 && n % 100 != 0;
    }

    /// <summary>
    /// Determines whether the given date is valid, must be after January 1, 1800.
    /// </summary>
    /// <param name="value">The input date string</param>
    /// <returns>True if valid, False if not</returns>
    public static bool IsDateTime(this string value)
    {
        _ = DateTime.TryParse(value, out var result);
        return result.CompareTo(DateTime.Parse("1800-1-1")) > 0;
    }

    /// <summary>
    /// Determines whether the given time is within the specified range.
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="start">The start time</param>
    /// <param name="end">The end time</param>
    /// <param name="mode">The mode</param>
    /// <returns></returns>
    public static bool In(this in DateTime dateTime, DateTime start, DateTime end, RangeMode mode = RangeMode.Close)
    {
        return mode switch
        {
            RangeMode.Open => start < dateTime && end > dateTime,
            RangeMode.Close => start <= dateTime && end >= dateTime,
            RangeMode.OpenClose => start < dateTime && end >= dateTime,
            RangeMode.CloseOpen => start <= dateTime && end > dateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    /// <summary>
    /// Returns the first and last day of the given month.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="month">The month</param>
    /// <param name="firstDay">The first day</param>
    /// <param name="lastDay">The last day</param>
    public static void GetDateFormat(this DateTime _, int month, out string firstDay, out string lastDay)
    {
        var year = DateTime.Now.Year + month / 12;
        if (month != 12)
        {
            month %= 12;
        }

        switch (month)
        {
            case 1:
                firstDay = DateTime.Now.ToString($"{year}-0{month}-01");
                lastDay = DateTime.Now.ToString($"{year}-0{month}-31");
                break;

            case 2:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.IsLeapYear(DateTime.Now.Year) ? DateTime.Now.ToString(year + "-0" + month + "-29") : DateTime.Now.ToString(year + "-0" + month + "-28");
                break;

            case 3:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString("yyyy-0" + month + "-31");
                break;

            case 4:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                break;

            case 5:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                break;

            case 6:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                break;

            case 7:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                break;

            case 8:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-31");
                break;

            case 9:
                firstDay = DateTime.Now.ToString(year + "-0" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-0" + month + "-30");
                break;

            case 10:
                firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-" + month + "-31");
                break;

            case 11:
                firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-" + month + "-30");
                break;

            default:
                firstDay = DateTime.Now.ToString(year + "-" + month + "-01");
                lastDay = DateTime.Now.ToString(year + "-" + month + "-31");
                break;
        }
    }

    /// <summary>
    /// Returns the last day of a given year and month.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="year">The year</param>
    /// <param name="month">The month</param>
    /// <returns>The day</returns>
    public static int GetMonthLastDate(this DateTime _, int year, int month)
    {
        var lastDay = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
        var day = lastDay.Day;
        return day;
    }

    /// <summary>
    /// Gets the number of hours between two given times.
    /// </summary>
    /// <param name="dtStart">The start time</param>
    /// <param name="dtEnd">The end time</param>
    /// <returns>The hour difference</returns>
    public static string GetTimeDelay(this in DateTime dtStart, DateTime dtEnd)
    {
        var lTicks = (dtEnd.Ticks - dtStart.Ticks) / 10000000;
        var sTemp = (lTicks / 3600).ToString().PadLeft(2, '0') + ":";
        sTemp += (lTicks % 3600 / 60).ToString().PadLeft(2, '0') + ":";
        sTemp += (lTicks % 3600 % 60).ToString().PadLeft(2, '0');
        return sTemp;
    }

    /// <summary>
    /// Gets an 8-digit integer representing the current time.
    /// </summary>
    /// <param name="value">The current date-time object</param>
    /// <returns>An 8-digit integer representing the time</returns>
    public static string GetDateString(this in DateTime value)
    {
        return value.Year + value.Month.ToString().PadLeft(2, '0') + value.Day.ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// Returns the time difference between two dates.
    /// </summary>
    /// <param name="dateTime1">The first date/time</param>
    /// <param name="dateTime2">The second date/time</param>
    /// <returns>The time difference</returns>
    public static string DateDiff(this in DateTime dateTime1, in DateTime dateTime2)
    {
        string dateDiff;
        var ts = dateTime2 - dateTime1;
        if (ts.Days >= 1)
        {
            dateDiff = dateTime1.Month + "月" + dateTime1.Day + "日";
        }
        else
        {
            dateDiff = ts.Hours > 1 ? ts.Hours + "小时前" : ts.Minutes + "分钟前";
        }

        return dateDiff;
    }

    /// <summary>
    /// Calculates the time difference between two times.
    /// </summary>
    /// <param name="beginTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <returns>The time difference</returns>
    public static string GetDiffTime(this in DateTime beginTime, in DateTime endTime)
    {
        var strResout = string.Empty;
        // Gets the time interval in seconds between two times.
        var span = endTime.Subtract(beginTime);
        var sec = Convert.ToInt32(span.TotalSeconds);
        var minutes = 1 * 60;
        var hours = minutes * 60;
        var day = hours * 24;
        var month = day * 30;
        var year = month * 12;

        // Reminder time: returns 1 if the time has arrived, otherwise returns 0.
        if (sec > year)
        {
            strResout += sec / year + "年";
            sec %= year; // Remaining
        }

        if (sec > month)
        {
            strResout += sec / month + "月";
            sec %= month;
        }

        if (sec > day)
        {
            strResout += sec / day + "天";
            sec %= day;
        }

        if (sec > hours)
        {
            strResout += sec / hours + "小时";
            sec %= hours;
        }

        if (sec > minutes)
        {
            strResout += sec / minutes + "分";
            sec %= minutes;
        }

        strResout += sec + "秒";
        return strResout;
    }

    /// <summary>
    /// ToDateString("yyyy-MM-dd")
    /// </summary>
    /// <param name="value">value</param>
    /// <returns></returns>
    public static string ToStandardDateString(this DateTime value)
    {
        return value.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// ToTimeString("yyyy-MM-dd HH:mm:ss")
    /// </summary>
    /// <param name="value">datetime</param>
    /// <returns></returns>
    public static string ToStandardTimeString(this DateTime value)
    {
        return value.ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// ToTimeString("yyyy-MM-dd HH:mm:ss:fffffff")
    /// </summary>
    /// <param name="value">datetime</param>
    /// <returns></returns>
    public static string ToStandardFullTimeString(this in DateTime value) => value.ToString("yyyy-MM-dd HH:mm:ss:fffffff");

    /// <summary>
    ///     Converts a time to the time in a particular time zone.
    /// </summary>
    /// <param name="value">The value and time to convert.</param>
    /// <param name="destinationTimeZone">The time zone to convert  to.</param>
    /// <returns>The value and time in the destination time zone.</returns>
    public static DateTime ConvertTime(this DateTime value, TimeZoneInfo destinationTimeZone)
    {
        return TimeZoneInfo.ConvertTime(value, destinationTimeZone);
    }

    /// <summary>
    ///     Converts a time from one time zone to another.
    /// </summary>
    /// <param name="value">The value and time to convert.</param>
    /// <param name="sourceTimeZone">The time zone of .</param>
    /// <param name="destinationTimeZone">The time zone to convert  to.</param>
    /// <returns>
    ///     The value and time in the destination time zone that corresponds to the  parameter in the source time zone.
    /// </returns>
    public static DateTime ConvertTime(this DateTime value, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
    {
        return TimeZoneInfo.ConvertTime(value, sourceTimeZone, destinationTimeZone);
    }

    /// <summary>
    ///     A DateTime extension method that query if 'value' is a week day.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if 'value' is a week day, false if not.</returns>
    public static bool IsWeekDay(this DateTime value)
    {
        return !(value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday);
    }

    /// <summary>
    ///     A DateTime extension method that query if 'value' is a week day.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>true if 'value' is a week day, false if not.</returns>
    public static bool IsWeekendDay(this DateTime value)
    {
        return value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
    }

    /// <summary>
    ///     A DateTime extension method that ages the given this.
    /// </summary>
    /// <param name="value">The value to act on.</param>
    /// <returns>An int.</returns>
    public static int GetAge(this DateTime value)
    {
        if (DateTime.Today.Month < value.Month ||
            DateTime.Today.Month == value.Month &&
            DateTime.Today.Day < value.Day)
        {
            return DateTime.Today.Year - value.Year - 1;
        }
        return DateTime.Today.Year - value.Year;
    }
}

/// <summary>
/// Range mode
/// </summary>
public enum RangeMode
{
    /// <summary>
    /// Open interval
    /// </summary>
    Open,

    /// <summary>
    /// Closed interval
    /// </summary>
    Close,

    /// <summary>
    /// Left open, right closed interval
    /// </summary>
    OpenClose,

    /// <summary>
    /// Left closed, right open interval
    /// </summary>
    CloseOpen
}

