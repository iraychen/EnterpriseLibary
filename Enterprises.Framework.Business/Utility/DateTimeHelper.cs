using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enterprises.Framework.Utility
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取日期yyyy-MM-dd字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static string ToShortDateString(this DateTime? dateValue)
        {
            try
            {
                if (dateValue == null)
                {
                    return "";
                }

                return dateValue.GetValueOrDefault().ToString("yyyy-MM-dd");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取日期yyyy-MM-dd HH:mm:ss字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static string ToDateTimeString(this DateTime? dateValue)
        {
            try
            {
                if (dateValue == null)
                {
                    return "";
                }

                return dateValue.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取日期HH:mm:ss字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static string ToTimeString(this DateTime? dateValue)
        {
            try
            {
                if (dateValue == null)
                {
                    return "";
                }

                DateTime dt = dateValue.GetValueOrDefault();
                return string.Format("{0}:{1}:{2}", dt.Hour, dt.Minute, dt.Second);
            }
            catch
            {
                return string.Empty;
            }


        }

        /// <summary>
        /// 获取日期yyyy-MM-dd HH:mm字符串
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static string ToDateTimeNoSecondsString(this DateTime? dateValue)
        {
            try
            {
                if (dateValue == null)
                {
                    return "";
                }

                return dateValue.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm");
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取今天起始时间（yyyy-MM-dd 00:00:00）
        /// </summary>
        /// <returns></returns>
        public static DateTime GetTodayStartTime()
        {
            DateTime dt = DateTime.Now;
            var str = string.Format("{0}-{1}-{2} 00:00:00", dt.Year, dt.Month, dt.Day);
            return Convert.ToDateTime(str);
        }

        /// <summary>
        /// 获取这周的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetWeekFristDay(this DateTime dateTime)
        {
            return dateTime.AddDays(Convert.ToDouble((0 - Convert.ToInt16(dateTime.DayOfWeek))));

        }

        /// <summary>
        /// 获取这周的最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetWeekLastDay(this DateTime dateTime)
        {
            return dateTime.AddDays(Convert.ToDouble((6 - Convert.ToInt16(dateTime.DayOfWeek))));
        }

        private static readonly string[] Day = new string[] {"星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"};

        /// <summary>
        /// 获取中午星期几
        /// </summary>
        /// <returns></returns>
        public static string ToCnWeekDay(this DateTime dateTime)
        {
            //return CaculateWeekDay(dateTime.Year, dateTime.Month, dateTime.Day);
            return Day[Convert.ToInt16(dateTime.DayOfWeek)];
        }

        /// <summary>
        /// 获取当月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthFristDay(this DateTime dateTime)
        {
            var dateStr = dateTime.ToString("yyyy-MM-01");
            return DateTime.Parse(dateStr);
        }

        /// <summary>
        /// 获取当月最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthLastDay(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-MM-01")).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 获取当前时间当年第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYearFristDay(this DateTime dateTime)
        {
            var dateStr = dateTime.ToString("yyyy-01-01");
            return DateTime.Parse(dateStr);
        }

        /// <summary>
        /// 获取当前时间当年最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetYearLastDay(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.ToString("yyyy-01-01")).AddYears(1).AddDays(-1);
        }

        /// <summary>
        /// 获取当前时间季度第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetQuarterFristDay(this DateTime dateTime)
        {
            var dateStr = dateTime.AddMonths(0 - ((dateTime.Month - 1)%3)).ToString("yyyy-MM-01");
            return DateTime.Parse(dateStr);
        }

        /// <summary>
        /// 获取当前时间季度最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetQuarterLastDay(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.AddMonths(3 - ((dateTime.Month - 1)%3)).ToString("yyyy-MM-01")).AddDays(-1);
        }

        /// <summary>
        /// 获取时间月份的天数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetMonthDays(this DateTime dateTime)
        {
            int m = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            return m;
        }


        /// <summary>
        /// 获取某月的实际工作日(即不包括周六日)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetDays(this DateTime dateTime)
        {
            int m = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            int mm = 0;
            for (int i = 1; i <= m; i++)
            {
                DateTime date = Convert.ToDateTime(dateTime.Year + "-" + dateTime.Month + "-" + i);
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Friday:
                        mm = mm + 1;
                        break;
                }
            }
            return mm;
        }

        /// <summary>
        /// 获取两个时间段的实际工作日(即不包括周六日)
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        public static int GetDays(DateTime date1, DateTime date2)
        {
            string m = DateDiff(DateInterval.Day, date1, date2).ToString("f0");
            int mm = 0;
            for (int i = 0; i <= Convert.ToInt32(m); i++)
            {
                DateTime date = Convert.ToDateTime(date1.AddDays(i));
                switch (date.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                    case DayOfWeek.Thursday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Friday:
                        mm = mm + 1;
                        break;
                }
            }
            return mm;
        }

        /// <summary>
        /// 获取两个时间的间隔
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static long DateDiff(DateInterval interval, DateTime startDate, DateTime endDate)
        {
            long lngDateDiffValue = 0;
            var ts = new TimeSpan(endDate.Ticks - startDate.Ticks);
            switch (interval)
            {
                case DateInterval.Second:
                    lngDateDiffValue = (long) ts.TotalSeconds;
                    break;
                case DateInterval.Minute:
                    lngDateDiffValue = (long) ts.TotalMinutes;
                    break;
                case DateInterval.Hour:
                    lngDateDiffValue = (long) ts.TotalHours;
                    break;
                case DateInterval.Day:
                    lngDateDiffValue = (long) ts.Days;
                    break;
                case DateInterval.Week:
                    lngDateDiffValue = (long) (ts.Days/7);
                    break;
                case DateInterval.Month:
                    lngDateDiffValue = (long) (ts.Days/30);
                    break;
                case DateInterval.Quarter:
                    lngDateDiffValue = (long) ((ts.Days/30)/3);
                    break;
                case DateInterval.Year:
                    lngDateDiffValue = (long) (ts.Days/365);
                    break;
            }

            return (lngDateDiffValue);
        }

        /// <summary>
        /// 获得本周的周六和周日
        /// </summary>
        /// <param name="date"></param>
        /// <param name="firstdate"></param>
        /// <param name="lastdate"></param>
        public static void ConvertDateToWeek(DateTime date, out DateTime firstdate, out DateTime lastdate)
        {
            DateTime first = DateTime.Now;
            DateTime last = DateTime.Now;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    first = date.AddDays(-1);
                    last = date.AddDays(5);
                    break;
                case DayOfWeek.Tuesday:
                    first = date.AddDays(-2);
                    last = date.AddDays(4);
                    break;
                case DayOfWeek.Wednesday:
                    first = date.AddDays(-3);
                    last = date.AddDays(3);
                    break;
                case DayOfWeek.Thursday:
                    first = date.AddDays(-4);
                    last = date.AddDays(2);
                    break;
                case DayOfWeek.Friday:
                    first = date.AddDays(-5);
                    last = date.AddDays(1);
                    break;
                case DayOfWeek.Saturday:
                    first = date.AddDays(-6);
                    last = date;
                    break;
                case DayOfWeek.Sunday:
                    first = date;
                    last = date.AddDays(6);
                    break;
            }

            firstdate = first;
            lastdate = last;
        }

        /// <summary>
        /// 获得当前日期是该年度的第几周
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeeks(this DateTime dateTime)
        {
            int weeks = dateTime.DayOfYear / 7 + 1;
            return weeks;
        }

    }
}

/// <summary>
/// 时间间隔
/// </summary>
public enum DateInterval
{
    /// <summary>
    /// 秒
    /// </summary>
    Second,
    /// <summary>
    /// 分钟
    /// </summary>
    Minute,
    /// <summary>
    /// 小时
    /// </summary>
    Hour,
    /// <summary>
    /// 天
    /// </summary>
    Day,
    /// <summary>
    /// 周
    /// </summary>
    Week,
    /// <summary>
    /// 月份
    /// </summary>
    Month,
    /// <summary>
    /// 季度
    /// </summary>
    Quarter,
    /// <summary>
    /// 年
    /// </summary>
    Year,
}

