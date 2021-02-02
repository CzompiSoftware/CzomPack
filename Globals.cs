using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
#if NETCOREAPP3_1 || NET5_0
using System.Text.Json;
#endif
using System.Threading.Tasks;

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
