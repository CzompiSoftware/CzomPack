using System.IO;
using System.Text;

namespace CzomPack.IO
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
