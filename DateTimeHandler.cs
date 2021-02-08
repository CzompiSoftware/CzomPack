using System;
using System.Globalization;

namespace CzomPack
{
    public class DateTimeHandler
    {
        /**
         * <summary>Nap számának lekérdezése.</summary>
         * <example>16</example>
         * <param name="dateTime">Date</param>
         */
        public static int GetDayNumber(DateTime dateTime) => dateTime.Day;

        /// <summary>
        /// Get the day of week from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With day number in the week</returns>
        public static int GetDayId(DateTime dateTime) => (int)dateTime.DayOfWeek;

        /**
         * <summary>Nap nevének lekérdezése.</summary>
         * <example>Szombat</example>
         * <param name="dateTime">Dátum</param>
         * <param name="culture">Culture code, default: <b>hu-HU</b></param>
         */
        public static string GetDayName(DateTime dateTime, string culture = "hu-HU") => dateTime.ToString("dddd", CultureInfo.CreateSpecificCulture(culture)).Substring(0, 1).ToUpper() + dateTime.ToString("dddd", CultureInfo.CreateSpecificCulture(culture)).Substring(1);

        /*
         * <summary>Hét számából  a hét első napjának (hétfő) lekérdezése.</summary>
         * <example>2019-08-12</example>
         * <param name="currentWeekNumber">Hét száma</param>
         */
        /// <summary>
        /// Get the first day of the week based on <paramref name="currentWeekNumber"/>
        /// </summary>
        /// <param name="currentWeekNumber">Number of the week</param>
        /// <returns>With the <see cref="DateTime"/> based on week number.</returns>
        public static DateTime WeekToDate(int currentWeekNumber)
        {
            return new DateTime(DateTime.Now.Year, 1, 1).AddDays(7 * (currentWeekNumber - 1) - 1);
        }

        /**
         * <summary>Formázott dátum lekérdezése.</summary>
         * <example>2019-03-16</example>
         * <param name="dateTime">Dátum</param>
         */
        /// <summary>
        /// Get the formatted date from <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">Date</param>
        /// <returns>With formatted date.<br/>Example: <c>2019-08-12</c></returns>
        public static String GetFormattedDate(DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        /**
         * <summary>Formázott dátum és idő lekérdezése.</summary>
         * <example>2019. 03. 16. 11:51</example>
         * <param name="dateTime">Dátum</param>
         */
        /// <returns>With formatted date.<br/>Example: <c>2019. 03. 16. 11:51</c></returns>
        public static String GetFormattedDateTime(DateTime dateTime) => dateTime.ToString("yyyy. MM. dd HH:mm");

        /**
         * <summary>Formázott idő lekérdezése.</summary>
         * <example>11:51:28</example>
         * <param name="dateTime">Dátum</param>
         */
        public static String GetFormattedTime(DateTime dateTime) => dateTime.ToString("HH:mm:ss");

        /**
         * <summary>Hét számának lekérése.</summary>
         * <example>11</example>
         * <param name="dateTime">Dátum</param>
         */
        public static int GetWeekNumber(DateTime dateTime)
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