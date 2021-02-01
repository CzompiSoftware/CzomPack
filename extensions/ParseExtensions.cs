using hu.czompisoftware.libraries.io;
using System;
using System.IO;
using System.Text;
#if NET5_0 || NETCOREAPP3_1
using System.Text.Json;
#else 
using Newtonsoft.Json;
#endif
using System.Xml;
using System.Xml.Serialization;

namespace hu.czompisoftware.libraries.extensions
{
    public static class ParseExtensions
    {
        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


        public static T ParseXML<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document, IgnoreWhitespace = true });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }
        

        public static string ToXMLString<T>(this T @this) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(@this.GetType());
            
            using (Utf8StringWriter textWriter = new Utf8StringWriter())
            {
                xmlSerializer.Serialize(textWriter, @this);
                return textWriter.ToString();
            }
        }

        public static T ParseJSON<T>(this string @this)
        {
#if NET5_0 || NETCOREAPP3_1
            return JsonSerializer.Deserialize<T>(@this.Trim());
#else
            return JsonConvert.DeserializeObject<T>(@this.Trim());
#endif
        }
    }
}