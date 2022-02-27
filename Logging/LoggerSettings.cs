using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CzomPack.Logging
{
    public class LoggerSettings
    {
        public static LogType MinimumLogLevel { get; set; } = LogType.Verbose;
        public static LogType MinimumFileLogLevel { get; set; } = LogType.Info;
        public static LogType MinimumConsoleLogLevel { get; set; } = LogType.Info;
    }
}
