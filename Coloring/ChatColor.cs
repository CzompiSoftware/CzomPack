using System;
using System.Drawing;

namespace CzomPack.Coloring
{

    ///TODO: Add docs to ChatColor
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public struct ChatColor
    {

        #region Properties

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color Color { get; }

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ConsoleColor? ConsoleColor { get; }

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char Code { get; }

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Name { get; }
        #endregion

        private ChatColor(char code, string name, Color? color = null, ConsoleColor? consoleColor = null)
        {
            this.Code = code;
            this.Name = name;
            if (color.HasValue)
            {
                this.Color = color.Value;
            }
            else
            {
                this.Color = Color.Transparent;
            }
            if (consoleColor.HasValue)
            {
                this.ConsoleColor = consoleColor.Value;
            }
            else
            {
                this.ConsoleColor = null;
            }
        }

        #region Methods for properties

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public char ToCode() => Code;

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Color ToColor() => Color;

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToName() => Name;

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.ConsoleColor? ToConsoleColor() => ConsoleColor;
        #endregion

        #region FromCode
        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ChatColor FromCode(string code)
        {
            var code_ = Char.ToLower(code.Replace("&", "").Replace("§", "")[0]);
            return FromCode(code_);
        }

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ChatColor FromCode(char code)
        {
            return code switch
            {
                '0' => Black,
                '1' => DarkBlue,
                '2' => DarkGreen,
                '3' => DarkCyan,
                '4' => DarkRed,
                '5' => Purple,
                '6' => Gold,
                '7' => Gray,
                '8' => DarkGray,
                '9' => Blue,
                'a' => BrightGreen,
                'b' => Cyan,
                'c' => Red,
                'd' => Pink,
                'e' => Yellow,
                'f' => White,
                'r' => Reset,
                _ => Reset // Maybe add an exception?
            };
        }
        #endregion


        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"§{Code}";

        #region Colors
        // 0-9

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Black = new ChatColor('0', "black", Color.FromArgb(0, 0, 0), System.ConsoleColor.Black);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor DarkBlue = new ChatColor('1', "dark_blue", Color.FromArgb(0, 0, 42), System.ConsoleColor.DarkBlue);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor DarkGreen = new ChatColor('2', "dark_green", Color.FromArgb(0, 42, 0), System.ConsoleColor.DarkGreen);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor DarkCyan = new ChatColor('3', "dark_aqua", Color.FromArgb(0, 42, 42), System.ConsoleColor.DarkCyan);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor DarkRed = new ChatColor('4', "dark_red", Color.FromArgb(42, 0, 0), System.ConsoleColor.DarkRed);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Purple = new ChatColor('5', "dark_purple", Color.FromArgb(42, 0, 42), System.ConsoleColor.DarkMagenta);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Gold = new ChatColor('6', "gold", Color.FromArgb(42, 42, 0), System.ConsoleColor.DarkYellow);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Gray = new ChatColor('7', "gray", Color.FromArgb(42, 42, 42), System.ConsoleColor.Gray);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor DarkGray = new ChatColor('8', "dark_gray", Color.FromArgb(85, 85, 85), System.ConsoleColor.DarkGray);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Blue = new ChatColor('9', "blue", Color.FromArgb(85, 85, 255), System.ConsoleColor.Blue);

        // A-F

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor BrightGreen = new ChatColor('a', "green", Color.FromArgb(85, 255, 85), System.ConsoleColor.Green);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Cyan = new ChatColor('b', "aqua", Color.FromArgb(85, 255, 255), System.ConsoleColor.Cyan);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Red = new ChatColor('c', "red", Color.FromArgb(255, 85, 85), System.ConsoleColor.Red);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Pink = new ChatColor('d', "light_purple", Color.FromArgb(255, 85, 255), System.ConsoleColor.Magenta);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Yellow = new ChatColor('e', "yellow", Color.FromArgb(255, 255, 85), System.ConsoleColor.Yellow);

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor White = new ChatColor('f', "white", Color.FromArgb(255, 255, 255), System.ConsoleColor.White);
        #endregion

        #region Effects

        ///TODO: Add docs to ChatColor
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static readonly ChatColor Reset = new ChatColor('r', "reset");
        #endregion

    }
}
