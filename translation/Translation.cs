using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
#if NET5_0 || NETCOREAPP3_1
using System.Text.Json.Serialization;
#endif

namespace hu.czompisoftware.libraries.translation
{
    public class Translation
    {
        public Info Language { get; set; }

        public Dictionary<string, TranslationStruct> Translations { get; set; }
        public string GetTranslatedString(string key)
        {
            var elem = Translations.Where(x => x.Key == key).ToList();
            if (elem.Count == 0) return key;
            var response = elem.FirstOrDefault().Value.Translated ?? elem.FirstOrDefault().Value.Original;
            return response.Replace("%application_name%", ExecutingApplication.Name)
                .Replace("%application_version%", ExecutingApplication.Version)
                .Replace("%username%", UserPrincipal.Current.DisplayName);
        }
    }

    public class Info
    {
        public string Name { get; set; }

        public string EnglishName { get; set; }
    }

    public class TranslationStruct
    {
#if NET5_0 || NETCOREAPP3_1
        [JsonPropertyName("Comment")]
#endif
        public string Original { get; set; }

#if NET5_0 || NETCOREAPP3_1
        [JsonPropertyName("Value")]
#endif
#if NET5_0
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
#endif
        public string Translated { get; set; }
    }
}
