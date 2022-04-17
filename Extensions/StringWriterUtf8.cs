using System.IO;
using System.Text;

namespace CzomPack.Extensions
{
    internal class StringWriterUtf8 : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}