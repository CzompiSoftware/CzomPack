using System;
using System.Reflection;

namespace CzomPack.Translation
{
    internal class ExecutingApplication
    {
        internal static String Name
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Name;
            }
        }
        internal static String Version
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName().Version;
                return String.Format("{0}.{1}{2}", ver.Major, ver.Minor, ver.Minor == 0 && ver.Minor == ver.Build ? "" : "." + ver.Build);
            }
        }
    }
}