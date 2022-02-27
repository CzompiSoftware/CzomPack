using System;
using System.IO;

namespace CzomPack
{
    public class Settings
    {
        public static Application Application { get; set; } = new();

        public static string WorkingDirectory { get; set; } = Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.Name));
        public static Serilog.LoggerConfiguration GetLoggerConfiguration() => Globals.LoggerConfiguration;
    }
}
