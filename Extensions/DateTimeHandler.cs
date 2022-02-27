using System;
using System.Globalization;

namespace CzomPack.Extensions
{
    /// <summary>
    /// Extension methods for DateTime.
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Get the current day of the month.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With formatted date.<br/>Example: <c>16</c></returns>
        public static int GetDayNumber(this DateTime dateTime) => dateTime.Day;

        /// <summary>
        /// Get the day of week from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With day number in the week<br/>Example: <c>4</c></returns>
        public static int GetDayId(this DateTime dateTime) => (int)dateTime.DayOfWeek;

        /// <summary>
        /// Get the name of the current day in the month.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <param name="culture">Culture code, default: <b>en-US</b></param>
        /// <returns>With the name of the day.<br/>Example: <c>Thursday</c></returns>
        public static string GetDayName(this DateTime dateTime, string culture = "en-US") => dateTime.ToString("dddd", CultureInfo.CreateSpecificCulture(culture));

        /// <summary>
        /// Get the first day of the week based on <paramref name="currentWeekNumber"/>
        /// </summary>
        /// <param name="currentWeekNumber">Number of the week</param>
        /// <returns>With the <see cref="DateTime"/> based on week number.</returns>
        public static DateTime WeekToDate(this int currentWeekNumber)
        {
            return new DateTime(DateTime.Now.Year, 1, 1).AddDays(7 * (currentWeekNumber - 1) - 1);
        }

        /// <summary>
        /// Get the formatted date from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With formatted date.<br/>Example: <c>2021-02-11</c></returns>
        public static String GetFormattedDate(this DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        /// <summary>
        /// Get the formatted date and time from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With formatted date and time.<br/>Example: <c>2021. 02. 11. 14:05</c></returns>
        public static String GetFormattedDateTime(this DateTime dateTime) => dateTime.ToString("yyyy. MM. dd. HH:mm");

        /// <summary>
        /// Get the formatted time from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With formatted time.<br/>Example: <c>14:05:38</c></returns>
        public static String GetFormattedTime(this DateTime dateTime) => dateTime.ToString("HH:mm:ss");

        /// <summary>
        /// Get week number from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With week number.<br/>Example: <c>6</c></returns>
        public static int GetWeekNumber(this DateTime dateTime)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(dateTime);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                dateTime = dateTime.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}