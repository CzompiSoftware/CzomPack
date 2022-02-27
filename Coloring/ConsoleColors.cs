using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzomPack.Coloring
{
    public class ConsoleColors
    {

        #region Foreground colors
        public class Foreground
        {
            public static string Black => "\u001b[30m";
            public static string DarkRed => "\u001b[31m";
            public static string DarkGreen => "\u001b[32m";
            public static string DarkYellow => "\u001b[33m";
            public static string DarkBlue => "\u001b[34m";
            public static string DarkMagenta => "\u001b[35m";
            public static string DarkCyan => "\u001b[36m";
            public static string LightGray => "\u001b[37m";
            public static string DarkGray => "\u001b[90m";
            public static string LightRed => "\u001b[91m";
            public static string LightGreen => "\u001b[92m";
            public static string LightYellow => "\u001b[93m";
            public static string LightBlue => "\u001b[94m";
            public static string LightMagenta => "\u001b[95m";
            public static string LightCyan => "\u001b[96m";
            public static string White => "\u001b[97m";

        }
        #endregion

        #region Background colors
        public class Background
        {
            public static string Black => "\u001b[40m";
            public static string DarkRed => "\u001b[41m";
            public static string DarkGreen => "\u001b[42m";
            public static string DarkYellow => "\u001b[43m";
            public static string DarkBlue => "\u001b[44m";
            public static string DarkMagenta => "\u001b[45m";
            public static string DarkCyan => "\u001b[46m";
            public static string LightGray => "\u001b[47m";
            public static string DarkGray => "\u001b[100m";
            public static string LightRed => "\u001b[101m";
            public static string LightGreen => "\u001b[102m";
            public static string LightYellow => "\u001b[103m";
            public static string LightBlue => "\u001b[104m";
            public static string LightMagenta => "\u001b[105m";
            public static string LightCyan => "\u001b[106m";
            public static string White => "\u001b[107m";
        }
        #endregion
    }
}
