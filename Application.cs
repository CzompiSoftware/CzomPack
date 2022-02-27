using System.Linq;
using System.Reflection;

namespace CzomPack;

public class Application
{
    public Application(Assembly assembly = null)
    {
        Assembly = assembly ?? Assembly.GetExecutingAssembly();
    }

    public Assembly Assembly { get; internal set; }

    public string Name
    {
        get
        {
            return Assembly.GetName().Name;
        }
    }
    public string Namespace
    {
        get
        {
            return Assembly.GetTypes().First(x=> !(x.Namespace.StartsWith("Microsoft") || x.Namespace.StartsWith("System"))).Namespace;
        }
    }
    public string Version
    {
        get
        {
            var ver = Assembly.GetName().Version;
            return $"{ver.Major}.{ver.Minor}{(ver.Minor == 0 && ver.Minor == ver.Build ? "" : "." + ver.Build)}";
        }
    }
}
