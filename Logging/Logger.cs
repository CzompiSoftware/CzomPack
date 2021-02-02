using CzomPack.Coloring;
using CzomPack.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CzomPack.Logging
{
    public class Logger
    {
        public static void Info(String msg, Boolean NotifyUser = false, Boolean Debug = false, Boolean Console = true)
        {
            WriteLine(msg, LogType.Info, NotifyUser, Debug, Console);
        }

        public static void Warning(String msg, Boolean NotifyUser = false, Boolean Debug = false, Boolean Console = true)
        {
            WriteLine(msg, LogType.Warning, NotifyUser, Debug, Console);
        }

        public static void Error(String msg, Boolean NotifyUser = false, Boolean Debug = false, Boolean Console = true)
        {
            WriteLine(msg, LogType.Error, NotifyUser, Debug, Console);
        }

        public static void FatalError(String msg, Boolean NotifyUser = false, Boolean Debug = false, Boolean Console = true)
        {
            WriteLine(msg, Type: LogType.FatalError, NotifyUser: NotifyUser, Debug, Console);
        }

        #region WriteLine - Global write function
        public static void WriteLine(String message, LogType Type = LogType.Info, Boolean NotifyUser = false, Boolean Debug = false, Boolean WriteToConsole = false, string Prefix = null)
        {
            String file = Assembly.GetEntryAssembly().GetName().Name.ToLower() + "-{date}.log";
            try
            {
                /*lock (_lock)
                {*/

                #region Prefix
                var prefix = $"[{DateTime.Now:s}] ";

                #region Log type
                prefix += Type switch
                {
                    LogType.Debug => ChatColor.DarkGreen,
                    LogType.Info => ChatColor.Blue,
                    LogType.Warning => ChatColor.Gold,
                    LogType.Error => ChatColor.Red,
                    LogType.FatalError => ChatColor.DarkRed,
                    _ => ChatColor.Gray,
                };

                var currPrefix = Type switch
                {
                    LogType.Info => "[Info]",
                    LogType.Debug => "[Debug]",
                    LogType.Warning => "[Warning]",
                    LogType.Error => "[Error]",
                    LogType.FatalError => "[Fatal Error]",
                    _ => "[????]",
                };

                prefix += currPrefix;
                prefix += " ";
                //prefix += string.Concat(Enumerable.Repeat(" ", 13 - currPrefix.Length + 1));
                #endregion

                #region Prefix based on calling method
                if (string.IsNullOrEmpty(Prefix))
                {
                    MethodBase currmethod = null;
#if NET45 || NET35
                    var sf = (List<StackFrame>)new System.Diagnostics.StackTrace().GetFrames().ToList();
                    currmethod = (sf[2]).GetMethod();
#elif NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2 || NETCOREAPP3_0 || NETCOREAPP3_1 || NET48 || NET462 || NET461 || NET5_0
                    currmethod = ((StackFrame)new System.Diagnostics.StackTrace().GetFrames().ToList().Skip(2).FirstOrDefault()).GetMethod();
#endif
                    var currentClass = $"{currmethod.DeclaringType.FullName}";
                    var paramLst = currmethod.GetParameters().Select(x => $"{ChatColor.Blue}{x.ParameterType} {ChatColor.Gray}{x.Name}").ToList();
                    if (Debug) currentClass += $".{currmethod.Name}({ChatColor.Reset}{string.Join($"{ChatColor.Gray}, {ChatColor.Reset}", paramLst)}{ChatColor.DarkGray})";
                    prefix += $"{ChatColor.Yellow}[{Assembly.GetEntryAssembly().GetName().Name}] {ChatColor.DarkGray}{currentClass}{ChatColor.Reset}: {ChatColor.White}";
                }
                else
                #endregion
                // This is here because of weird formatting 

#if NET45
                if (Prefix.Split('/').Length > 0)
#else
                if (Prefix.Split("/").Length > 0)
#endif
                {
                    if (Type == LogType.Debug || Type == LogType.Error)
                        prefix += $"{""}[{Prefix}] ";
                    else
                        prefix += $"{"",1}[{Prefix}] ";
                }
                else
                {
                    if (Prefix.Length >= 12)
                        prefix += $"{"",1}[{Prefix}] ";
                    else
                        prefix += $"[{Prefix}] ";
                }
                prefix.RenderColoredConsoleMessage();
                #endregion

                #region Message coloring & line break handling
                string[] msgLst;
#if NET45
                msgLst = message.Contains("§") ? message.Split('§') : new string[] { $"f{message}" };
#else
                msgLst = message.Contains("§") ? message.Split("§") : new string[] { $"f{message}" };
#endif
                if (msgLst.Length > 1 && msgLst[0].Length > 0 && msgLst[0][0] != '§') msgLst[0] = $"f{msgLst[0]}";
                foreach (var msg in msgLst)
                {
                    if (!string.IsNullOrEmpty(msg) && msg.Length > 1)
                    {
                        var colorStr = msg[0].ToString().ToLower()[0];

                        var color = ChatColor.FromCode(colorStr).ToConsoleColor();
                        string[] lines;
#if NET45
                        lines = msg.Contains("\n") ? msg.Split('\n').ToArray() : new string[] { msg };
#else
                        lines = msg.Contains("\n") ? msg.Split("\n").ToArray() : new string[] { msg };
#endif

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i > 0) Console.WriteLine();
                            if (i > 0) { prefix.RenderColoredConsoleMessage(); }
                            $"§{(i > 0 ? $"{colorStr}" : "")}{lines[i]}".RenderColoredConsoleMessage();
                        }
                    }
                }
                Console.ResetColor();
                Console.WriteLine();
                #endregion

                #region Writing log to file
                var dir = new FileInfo(file).Directory;
                if (!dir.Exists) dir.Create();
                System.IO.File.AppendAllLines(file.Replace("{date}", DateTimeHandler.GetFormattedDate(DateTime.Now).Replace("-", ".")), new List<String> { $"{prefix}{message.Replace("\n", "\n" + prefix)}".RemoveChatColor() });
                #endregion

                //}
                //if (Type == LogType.FatalError) Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void Debug(string message, bool NotifyUser = false, bool Console = false)
        {
            //#if DEBUG
            WriteLine(message, LogType.Debug, NotifyUser, true, Console);
            //#endif
        }
        #endregion
    }
}
