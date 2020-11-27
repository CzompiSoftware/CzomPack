using System;
using System.IO;
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
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
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