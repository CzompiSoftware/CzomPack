using CzomPack.Extensions;
using CzomPack.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Reflection;
#if NETCOREAPP3_1_OR_GREATER
using System.Text.Json;
#endif

namespace CzomPack
{
    internal class Globals
    {

        #region Directories
        internal static string LogsDirectory
        {
            get
            {
                var dir = Path.GetFullPath(Path.Combine(Settings.WorkingDirectory, "logs"));
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                return dir;
            }
        }
        #endregion

#if NETCOREAPP3_1_OR_GREATER
        #region Newtonsoft.Json -> System.Text.Json
        internal static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        #endregion
#endif
        internal static LoggerConfiguration LoggerConfiguration => new LoggerConfiguration()
            //.MinimumLevel.Verbose()
            .MinimumLevel.Is(LoggerSettings.MinimumLogLevel.ToLogEventLevel())
            // .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            // .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
            // .MinimumLevel.Override("System", LogEventLevel.Warning)
            // .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.File(
                Path.Combine(LogsDirectory, @$"{Settings.Application.Name}-{DateTime.Now:yyyy'.'MM'.'dd}.log"),
                outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
                fileSizeLimitBytes: 1_000_000,
#if RELEASE
                restrictedToMinimumLevel: LoggerSettings.MinimumFileLogLevel.ToLogEventLevel(),
#else
                restrictedToMinimumLevel: LogEventLevel.Verbose,
#endif
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}",
                theme: AnsiConsoleTheme.Code,
#if RELEASE
                restrictedToMinimumLevel: LoggerSettings.MinimumConsoleLogLevel.ToLogEventLevel()
#else
                restrictedToMinimumLevel: LogEventLevel.Verbose
#endif
            );

        internal static bool IsLoggerSet { get; set; } = false;

        internal static void SetupLogger()
        {
            if (!IsLoggerSet)
            {
                Log.Logger = LoggerConfiguration.CreateLogger();
                IsLoggerSet = true;
            }
        }
    }
}
