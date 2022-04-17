using CzomPack.IO;
using CzomPack.Logging;
using Serilog.Events;
using System;
using System.IO;
#if NETCOREAPP3_1_OR_GREATER
using System.Text.Json;
#else 
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;
#endif
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace CzomPack.Extensions
{
    public static class ParseExtensions
    {
        public static LogEventLevel ToLogEventLevel(this LogType @this) => @this switch
        {
            LogType.Error => LogEventLevel.Error,
            LogType.FatalError => LogEventLevel.Fatal,
            LogType.Warning => LogEventLevel.Warning,
            LogType.Debug => LogEventLevel.Debug,
            LogType.Verbose => LogEventLevel.Verbose,
            LogType.Info => LogEventLevel.Information,
            _ => throw new NotSupportedException("Unsupported Log type detected")
        };
        public static LogType ToLogType(this LogEventLevel @this) => @this switch
        {
            LogEventLevel.Error => LogType.Error, 
            LogEventLevel.Fatal => LogType.FatalError,
            LogEventLevel.Warning => LogType.Warning,
            LogEventLevel.Debug => LogType.Debug,
            LogEventLevel.Verbose => LogType.Verbose,
            LogEventLevel.Information => LogType.Info,
            _ => throw new NotSupportedException("Unsupported Log level detected")
        };
        public static Stream ToStream(this string @this)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(@this);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string FromStream(this Stream @this)
        {
            string content = null;
            var stream = new StreamReader(@this);
            if (stream.Read() != -1)
                content = stream.ReadToEnd();
            return content;
        }

        #region Xml
        public static T ToXml<T>(this string @this) where T : class
        {
            var reader = XmlReader.Create(@this.Trim().ToStream(), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return new XmlSerializer(typeof(T)).Deserialize(reader) as T;
        }

        public static string FromXml<T>(this T @this, bool writeIndented = true) where T : class
        {
            StringWriterUtf8 stringWriter = new();
            XmlSerializer serializer = new(@this.GetType());
            serializer.Serialize(stringWriter, @this);
            string output = stringWriter.ToString();
            stringWriter.Dispose();

            if (writeIndented)
            {
                stringWriter = new();
                XmlDocument document = new();
                document.LoadXml(output);
                XmlTextWriter writer = new(stringWriter);
                writer.Formatting = Formatting.Indented;
                document.Save(writer);
                output = stringWriter.ToString();
                writer.Close();
                writer.Dispose();
                stringWriter.Close();
                stringWriter.Dispose();
            }
            return output;
        }

        public static void ToXmlFile<T>(this T @this, string fileName) where T : class
        {
            File.WriteAllText(fileName, @this.FromXml());
        }
        #endregion

        #region Json
        public static T ToJson<T>(this string @this) where T : class
        {
#if NETCOREAPP3_0_OR_GREATER
            return JsonSerializer.Deserialize<T>(@this.Trim(), Globals.JsonSerializerOptions);
#else
		return JsonConvert.DeserializeObject<T>(@this.Trim());
#endif
        }
        public static string FromJson<T>(this T @this) where T : class
        {
#if NETCOREAPP3_0_OR_GREATER
            return JsonSerializer.Serialize(@this, Globals.JsonSerializerOptions);
#else
		return JsonConvert.SerializeObject(@this, Newtonsoft.Json.Formatting.Indented);
#endif
        }
        public static void ToJsonFile<T>(this T @this, string fileName) where T : class
        {
#if NETCOREAPP3_0_OR_GREATER
            File.WriteAllText(fileName, JsonSerializer.Serialize(@this, Globals.JsonSerializerOptions));
#else
		File.WriteAllText(fileName, JsonConvert.SerializeObject(@this, Newtonsoft.Json.Formatting.Indented));
#endif
        }
        #endregion
    }
}