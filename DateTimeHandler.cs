using System;
using System.Globalization;

namespace CzomPack
{
    public class DateTimeHandler
    {
        /**
         * <summary>Nap számának lekérdezése.</summary>
         * <example>16</example>
         * <param name="dateTime">Dátum</param>
         */
        public static int GetDayNumber(DateTime dateTime) => dateTime.Day;

        /**
         * <summary>Nap számának lekérdezése.</summary>
         * <example>1</example>
         * <param name="dateTime">Dátum</param>
         */
        public static int GetDayId(DateTime dateTime) => (int)dateTime.DayOfWeek;

        /**
         * <summary>Nap nevének lekérdezése.</summary>
         * <example>Szombat</example>
         * <param name="dateTime">Dátum</param>
         * <param name="culture">Országkód, alapértelmezett: <b>hu-HU</b></param>
         */
        public static string GetDayName(DateTime dateTime, string culture = "hu-HU") => dateTime.ToString("dddd", CultureInfo.CreateSpecificCulture(culture)).Substring(0, 1).ToUpper() + dateTime.ToString("dddd", CultureInfo.CreateSpecificCulture(culture)).Substring(1);

        /**
         * <summary>Hét számából  a hét első napjának (hétfő) lekérdezése.</summary>
         * <example>2019-08-12</example>
         * <param name="int">Hét száma</param>
         */
        public static DateTime WeekToDate(int currentWeekId)
        {
            return new DateTime(DateTime.Now.Year, 1, 1).AddDays(7 * (currentWeekId - 1) - 1);
        }

        /**
         * <summary>Formázott dátum lekérdezése.</summary>
         * <example>2019-03-16</example>
         * <param name="dateTime">Dátum</param>
         */
        public static String GetFormattedDate(DateTime dateTime) => dateTime.ToString("yyyy-MM-dd");

        /**
         * <summary>Formázott dátum és idő lekérdezése.</summary>
         * <example>2019. 03. 16. 11:51</example>
         * <param name="dateTime">Dátum</param>
         */
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