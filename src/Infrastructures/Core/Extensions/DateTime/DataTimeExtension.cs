using System.Diagnostics;
using System.Globalization;

namespace System;

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
    /// 获取某一年有多少周
    /// </summary>
    /// <param name="_"></param>
    /// <param name="year">年份</param>
    /// <returns>该年周数</returns>
    public static int GetWeekAmount(this DateTime _, int year)
    {
        var end = new DateTime(year, 12, 31); //该年最后一天
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday); //该年星期数
    }

    /// <summary>
    /// 返回年度第几个星期   默认星期日是第一天
    /// </summary>
    /// <param name="value">时间</param>
    /// <returns>第几周</returns>
    public static int WeekOfYear(this in DateTime value)
    {
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(value, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
    }

    /// <summary>
    /// 返回年度第几个星期
    /// </summary>
    /// <param name="date">时间</param>
    /// <param name="week">一周的开始日期</param>
    /// <returns>第几周</returns>
    public static int WeekOfYear(this in DateTime date, DayOfWeek week)
    {
        var gc = new GregorianCalendar();
        return gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, week);
    }

    /// <summary>
    /// 得到一年中的某周的起始日和截止日
    /// 年 nYear
    /// 周数 nNumWeek
    /// 周始 out dtWeekStart
    /// 周终 out dtWeekeEnd
    /// </summary>
    /// <param name="_"></param>
    /// <param name="nYear">年份</param>
    /// <param name="nNumWeek">第几周</param>
    /// <param name="dtWeekStart">开始日期</param>
    /// <param name="dtWeekeEnd">结束日期</param>
    public static void GetWeekTime(this DateTime _, int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
    {
        var dt = new DateTime(nYear, 1, 1);
        dt += new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
        dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
        dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
    }

    /// <summary>
    /// 得到一年中的某周的起始日和截止日    周一到周五  工作日
    /// </summary>
    /// <param name="_"></param>
    /// <param name="nYear">年份</param>
    /// <param name="nNumWeek">第几周</param>
    /// <param name="dtWeekStart">开始日期</param>
    /// <param name="dtWeekeEnd">结束日期</param>
    public static void GetWeekWorkTime(this DateTime _, int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
    {
        var dt = new DateTime(nYear, 1, 1);
        dt += new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
        dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
        dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1).AddDays(-2);
    }

    /// <summary>
    /// 返回相对于当前时间的相对天数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="relativeday">相对天数</param>
    public static string GetDateTime(this in DateTime value, int relativeday)
    {
        return value.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的秒数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalSeconds(this in DateTime value) => new DateTimeOffset(value).ToUnixTimeSeconds();

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的毫秒数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalMilliseconds(this in DateTime value) => new DateTimeOffset(value).ToUnixTimeMilliseconds();

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的微秒时间戳
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long GetTotalMicroseconds(this in DateTime value) => new DateTimeOffset(value).Ticks / 10;

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的纳秒时间戳
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long GetTotalNanoseconds(this in DateTime value) => new DateTimeOffset(value).Ticks * 100 + Stopwatch.GetTimestamp() % 100;

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的分钟数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalMinutes(this in DateTime value) => new DateTimeOffset(value).Offset.TotalMinutes;

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的小时数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalHours(this in DateTime value) => new DateTimeOffset(value).Offset.TotalHours;

    /// <summary>
    /// 获取该时间相对于1970-01-01 00:00:00的天数
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetTotalDays(this in DateTime value) => new DateTimeOffset(value).Offset.TotalDays;

    /// <summary>
    /// 返回本年有多少天
    /// </summary>
    /// <param name="_"></param>
    /// <param name="iYear">年份</param>
    /// <returns>本年的天数</returns>
    public static int GetDaysOfYear(this DateTime _, int iYear)
    {
        return IsRuYear(iYear) ? 366 : 365;
    }

    /// <summary>本年有多少天</summary>
    /// <param name="value">日期</param>
    /// <returns>本天在当年的天数</returns>
    public static int GetDaysOfYear(this in DateTime value)
    {
        //取得传入参数的年份部分，用来判断是否是闰年
        var n = value.Year;
        return IsRuYear(n) ? 366 : 365;
    }

    /// <summary>本月有多少天</summary>
    /// <param name="_"></param>
    /// <param name="iYear">年</param>
    /// <param name="month">月</param>
    /// <returns>天数</returns>
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

    /// <summary>本月有多少天</summary>
    /// <param name="vakye">日期</param>
    /// <returns>天数</returns>
    public static int GetDaysOfMonth(this in DateTime vakye)
    {
        //--利用年月信息，得到当前月的天数信息。
        return vakye.Month switch
        {
            1 => 31,
            2 => (IsRuYear(vakye.Year) ? 29 : 28),
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

    /// <summary>返回当前日期的星期名称</summary>
    /// <param name="value">日期</param>
    /// <returns>星期名称</returns>
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

    /// <summary>返回当前日期的星期编号</summary>
    /// <param name="value">日期</param>
    /// <returns>星期数字编号</returns>
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

    /// <summary>判断当前年份是否是闰年，私有函数</summary>
    /// <param name="value">年份</param>
    /// <returns>是闰年：True ，不是闰年：False</returns>
    private static bool IsRuYear(int value)
    {
        //形式参数为年份
        //例如：2003
        var n = value;
        return n % 400 == 0 || n % 4 == 0 && n % 100 != 0;
    }

    /// <summary>
    /// 判断是否为合法日期，必须大于1800年1月1日
    /// </summary>
    /// <param name="value">输入日期字符串</param>
    /// <returns>True/False</returns>
    public static bool IsDateTime(this string value)
    {
        _ = DateTime.TryParse(value, out var result);
        return result.CompareTo(DateTime.Parse("1800-1-1")) > 0;
    }

    /// <summary>
    /// 判断时间是否在区间内
    /// </summary>
    /// <param name="this"></param>
    /// <param name="start">开始</param>
    /// <param name="end">结束</param>
    /// <param name="mode">模式</param>
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
    ///  返回每月的第一天和最后一天
    /// </summary>
    /// <param name="_"></param>
    /// <param name="month">月份</param>
    /// <param name="firstDay">第一天</param>
    /// <param name="lastDay">最后一天</param>
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
    /// 返回某年某月最后一天
    /// </summary>
    /// <param name="_"></param>
    /// <param name="year">年份</param>
    /// <param name="month">月份</param>
    /// <returns>日</returns>
    public static int GetMonthLastDate(this DateTime _, int year, int month)
    {
        var lastDay = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
        var day = lastDay.Day;
        return day;
    }

    /// <summary>
    /// 获得一段时间内有多少小时
    /// </summary>
    /// <param name="dtStar">起始时间</param>
    /// <param name="dtEnd">终止时间</param>
    /// <returns>小时差</returns>
    public static string GetTimeDelay(this in DateTime dtStar, DateTime dtEnd)
    {
        var lTicks = (dtEnd.Ticks - dtStar.Ticks) / 10000000;
        var sTemp = (lTicks / 3600).ToString().PadLeft(2, '0') + ":";
        sTemp += (lTicks % 3600 / 60).ToString().PadLeft(2, '0') + ":";
        sTemp += (lTicks % 3600 % 60).ToString().PadLeft(2, '0');
        return sTemp;
    }

    /// <summary>
    /// 获得8位时间整型数字
    /// </summary>
    /// <param name="value">当前的日期时间对象</param>
    /// <returns>8位时间整型数字</returns>
    public static string GetDateString(this in DateTime value)
    {
        return value.Year + value.Month.ToString().PadLeft(2, '0') + value.Day.ToString().PadLeft(2, '0');
    }

    /// <summary>
    /// 返回时间差
    /// </summary>
    /// <param name="dateTime1">时间1</param>
    /// <param name="dateTime2">时间2</param>
    /// <returns>时间差</returns>
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
    /// 计算2个时间差
    /// </summary>
    /// <param name="beginTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>时间差</returns>
    public static string GetDiffTime(this in DateTime beginTime, in DateTime endTime)
    {
        var strResout = string.Empty;
        //获得2时间的时间间隔秒计算
        var span = endTime.Subtract(beginTime);
        var sec = Convert.ToInt32(span.TotalSeconds);
        var minutes = 1 * 60;
        var hours = minutes * 60;
        var day = hours * 24;
        var month = day * 30;
        var year = month * 12;

        //提醒时间,到了返回1,否则返回0
        if (sec > year)
        {
            strResout += sec / year + "年";
            sec %= year; //剩余
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
/// 区间模式
/// </summary>
public enum RangeMode
{
    /// <summary>
    /// 开区间
    /// </summary>
    Open,

    /// <summary>
    /// 闭区间
    /// </summary>
    Close,

    /// <summary>
    /// 左开右闭区间
    /// </summary>
    OpenClose,

    /// <summary>
    /// 左闭右开区间
    /// </summary>
    CloseOpen
}
