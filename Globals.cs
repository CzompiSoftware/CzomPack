using System;
using System.IO;
using System.Reflection;
#if NETCOREAPP3_1 || NET5_0
using System.Text.Json;
#endif

namespace CzomPack
{
    internal class Globals
    {
        public static string WorkingDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name);

        public static string ProxyFile => Path.Combine(WorkingDir, "proxy.json");

#if NETCOREAPP3_1 || NET5_0
        #region Newtonsoft.Json -> System.Text.Json
        public static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = false,
            IgnoreNullValues = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
        #endregion
#endif

    }
}
