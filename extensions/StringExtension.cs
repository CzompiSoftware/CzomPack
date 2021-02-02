using CzomPack.Coloring;
using CzomPack.Extensions.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CzomPack.Extensions
{
    public static class StringExtension
    {
        #region Playing with coloring
        public static void RenderColoredConsoleMessage(this string message, bool AddNewLine = false)
        {
            message = message.Replace("&", "§");
            string[] msgLst;
#if NET45
            msgLst = message.Contains("§") ? message.Split('§') : new string[] { $"r{message}" };
#else
            msgLst = message.Contains("§") ? message.Split("§") : new string[] { $"r{message}" };
#endif
            if (message[0] != '§' && msgLst.Length > 1) msgLst[0] = $"r{msgLst[0]}";
            foreach (var msg in msgLst)
            {
                if (!string.IsNullOrEmpty(msg) && msg.Length > 1)
                {
                    var colorStr = msg[0].ToString().ToLower()[0];
                    var consoleColor = ChatColor.FromCode(colorStr).ToConsoleColor();
                    if (colorStr.IsRealChatColor())
                    {
                        if (colorStr == 'r') Console.ResetColor();
                        else if (consoleColor.HasValue) Console.ForegroundColor = consoleColor.Value;
                    }
#if NET5_0 || NETCOREAPP3_1
                    Console.Write(colorStr.IsRealChatColor() ? msg[1..] : msg);
#else
                    Console.Write(colorStr.IsRealChatColor() ? msg.Substring(1) : msg);
#endif
                }
            }
            Console.ResetColor();
            if (AddNewLine) Console.WriteLine("");
        }
        public static bool IsRealChatColor(this string suspectedColor, bool skipPrefix = false) => (skipPrefix switch
        {
            true => new Regex("^([a-f]|r|o|m|n|k|l|[0-9])$"),
            false => new Regex("^([§|&])([a-f]|r|o|m|n|k|l|[0-9])$")
        }).IsMatch($"{suspectedColor.ToLower()}");

        public static bool IsRealChatColor(this char suspectedColor) => suspectedColor.ToString().ToLower().IsRealChatColor(true);

        public static string RemoveChatColor(this string coloredString) => new Regex("([§|&])([a-f]|r|o|m|n|k|l|[0-9])").Replace(coloredString, "");
        #endregion

        #region ContainsBunch
        public static bool ContainsBunch(this string @this, params string[] values) => @this.ContainsBunch(ContainsType.AllOfThem, StringComparison.OrdinalIgnoreCase, values);

        public static bool ContainsBunch(this string @this, StringComparison comparison, params string[] values) => @this.ContainsBunch(ContainsType.AllOfThem, comparison, values);
        
        public static bool ContainsBunch(this string @this, ContainsType ct, params string[] values) => @this.ContainsBunch(ct, StringComparison.OrdinalIgnoreCase, values);

        public static bool ContainsBunch(this string @this, ContainsType ct, StringComparison comparison, params string[] values) => ct switch
        {
#if NET45
            ContainsType.AllOfThem => !values.ToList().Select(x => comparison == StringComparison.OrdinalIgnoreCase || comparison == StringComparison.CurrentCultureIgnoreCase || comparison == StringComparison.InvariantCultureIgnoreCase ? @this.ToLower().Contains(x.ToLower()) : @this.Contains(x)).Contains(false),
#else
            ContainsType.AllOfThem => values.ToList().Select(x => @this.Contains(x, comparison)).Contains(true),
#endif
#if NET45
            ContainsType.OneOfThem => values.ToList().Select(x => comparison == StringComparison.OrdinalIgnoreCase || comparison == StringComparison.CurrentCultureIgnoreCase || comparison == StringComparison.InvariantCultureIgnoreCase ? @this.ToLower().Contains(x.ToLower()): @this.Contains(x)).Contains(true),
#else
            ContainsType.OneOfThem => values.ToList().Select(x => @this.Contains(x, comparison)).Contains(true),
#endif
            _ => throw new System.ArgumentException("Unsupported `ContainsType` selected! How could this possibly happen?!")
        };
        #endregion
    }
}
