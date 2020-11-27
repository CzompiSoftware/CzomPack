using hu.czompisoftware.libraries.coloring;
using hu.czompisoftware.libraries.extensions;
using hu.czompisoftware.libraries.general;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
#if NET5_0 || NETCOREAPP3_1
using System.Text.Json;
#else
using Newtonsoft.Json;
#endif
using System.Text.RegularExpressions;
using System.Threading;

namespace hu.czompisoftware.libraries.translation
{

    public class TranslationManager: LanguageInfo
    {

#region Properties
        internal static Assembly ParentAssembly { get; set; }

        private IList<string> LanguageFileNames { get; }
        private Dictionary<string, Translation> LanguageList { get; }

        protected LanguageInfo DefaultLangInfo { get; set; } = new LanguageInfo("en");
        protected LanguageInfo LangInfo = null;

        public Translation Current { get; internal set; }
        public Translation Default { get; internal set; }
        public string DefaultFileLoc { get; private set; }
#endregion

        public TranslationManager(Assembly parentAssembly, string langCode = "{default}")
        {
            langCode = langCode == "{default}" || langCode.ToLower() == DefaultLangInfo.Code.ToLower() ? null : langCode;
            TranslationManager.ParentAssembly = parentAssembly ?? Assembly.GetCallingAssembly();
            CultureInfo lang = null;
            //Stream stream = parentAssembly.GetManifestResourceStream(ResourceName);
            LanguageFileNames = GetResources(@"([Rl]esources\.[Ll]ang\..*\.json)$");
            LanguageList = LoadTranslationList();
            //lang = CultureInfo.GetCultureInfo(Globals.Configs.Config.Global.Locale);
            lang = CultureInfo.CurrentUICulture; var OSLangInfo = new LanguageInfo(lang.Name);
            LangInfo = new LanguageInfo(langCode ?? lang.Name);
            Code = LangInfo.Code;
            Name = LangInfo.Name;
            EnglishName = LangInfo.EnglishName;

            try
            {
                Default = FromCode(OSLangInfo);
            }
            catch (Exception)
            {
                Default = FromCode(DefaultLangInfo);
            }
            finally
            {
                Logger.Info($"Using {ChatColor.BrightGreen}{Default.Language.EnglishName}{ChatColor.White} as default language (based on system language).");
                Default.Language.EnglishName = Default.Translations["languages.osdefault"].Original;
                Default.Language.Name = Default.Translations["languages.osdefault"].Translated;
                
            }

            Current = FromCode(LangInfo);
            Logger.Info($"Using {ChatColor.BrightGreen}{Current.Language.EnglishName}{ChatColor.White} as current display language (based on selected language).");
        }

#region Loading stuff

#region GetTranslation
        protected Translation LoadTranslation(LanguageInfo lang, string assemblyLangLoc, LanguageInfo def = null, string assemblyDefLangLoc = null, bool isDefault = false)
        {
            ///DONE: Redo GetLanguage
            Translation res = null;
            var fileLangLoc = Path.Combine(Globals.WorkingDir, "Langs", $"{lang.Code}.json");
            var fileDefLangLoc = Path.Combine(Globals.WorkingDir, "Langs", $"en.json");
            if (!File.Exists(fileLangLoc))
            {
                res = LoadTranslationFromAssembly(lang, assemblyLangLoc, def, assemblyDefLangLoc, isDefault);
            }
            else
            {
                try
                {
                    res = LoadTranslationFromFile(lang, fileLangLoc, def, fileDefLangLoc, isDefault);
                }
                catch (MissingLanguageException)
                {
                    res = LoadTranslationFromAssembly(lang, assemblyLangLoc, def, assemblyDefLangLoc, isDefault);
                }
                catch
                {
                    Logger.Error("Cannot load any languages. If you're the application developer, please refer documentation to see more info about loading languages using our system.",true);
                }
            }
            return res;
        }
#endregion

#region LoadTranslationFromFile
        protected Translation LoadTranslationFromFile(LanguageInfo lang, string langLoc, LanguageInfo def = null, string defLangLoc = null, bool isDefault = false)
        {
            Translation language;
            try
            {
#if NET5_0 || NETCOREAPP3_1
                language = JsonSerializer.Deserialize<Translation>(File.ReadAllText(langLoc), Globals.JsonSerializerOptions);
#else
                language = JsonConvert.DeserializeObject<Translation>(File.ReadAllText(langLoc));
#endif
                language.Language.Name = lang.Name;
                language.Language.EnglishName = lang.EnglishName;
                Logger.Info($"{ChatColor.Gray}[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.BrightGreen}{lang.EnglishName}{ChatColor.Reset} language loaded.");
            }
            catch (Exception ex)
            {
                if (!isDefault) {
                    Logger.Warning($"§7[Language/{lang.Code}]{ChatColor.Reset} Cannot load §e{lang.EnglishName}{ChatColor.Reset} language. Falling back to default...");
                    if (ParentAssembly.IsDebugBuild()) Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.Red}{ex}!");
                    return LoadTranslationFromAssembly(def, defLangLoc, isDefault: true);
                } 
                else Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} Cannot load §e{lang.EnglishName}{ChatColor.Reset} language.");
                
                if (ParentAssembly.IsDebugBuild()) Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.Red}{ex}!");
#if NET5_0 || NETCOREAPP3_1
                throw new MissingLanguageException($"{JsonSerializer.Serialize(lang)}", ex);
#else
                throw new MissingLanguageException($"{JsonConvert.SerializeObject(lang)}", ex);
#endif
            }
            return language;
        }
#endregion
        
#region LoadTranslationFromAssembly
        protected Translation LoadTranslationFromAssembly(LanguageInfo lang, string langLoc, LanguageInfo def = null, string defLangLoc = null, bool isDefault = false)
        {
            Translation language;
            try
            {
                var resourceStream = ParentAssembly.GetManifestResourceStream(langLoc);

                var sr = new StreamReader(resourceStream);
                var text = sr.ReadToEnd();
#if NET5_0 || NETCOREAPP3_1
                language = JsonSerializer.Deserialize<Translation>(text, Globals.JsonSerializerOptions);
#else
                language = JsonConvert.DeserializeObject<Translation>(text);
#endif
                language.Language.Name = lang.Name;
                language.Language.EnglishName = lang.EnglishName;
                Logger.Info($"{ChatColor.Gray}[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.BrightGreen}{lang.EnglishName}{ChatColor.Reset} language loaded.");
            }
            catch (Exception ex)
            {
                if (!isDefault) {
                    Logger.Warning($"§7[Language/{lang.Code}]{ChatColor.Reset} Cannot load §e{lang.EnglishName}{ChatColor.Reset} language. Falling back to default...");
                    if (ParentAssembly.IsDebugBuild()) Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.Red}{ex}!");
                    return LoadTranslationFromAssembly(def, defLangLoc, isDefault: true);
                } 
                else Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} Cannot load §e{lang.EnglishName}{ChatColor.Reset} language.");
                
                if (ParentAssembly.IsDebugBuild()) Logger.Error($"§7[Language/{lang.Code}]{ChatColor.Reset} {ChatColor.Red}{ex}!");
#if NET5_0 || NETCOREAPP3_1
                throw new MissingLanguageException($"{JsonSerializer.Serialize(lang)}", ex);
#else
                throw new MissingLanguageException($"{JsonConvert.SerializeObject(lang)}", ex);
#endif
            }
            return language;
        }
#endregion

#region LoadLanguageList
        public Dictionary<String, Translation> LoadTranslationList()
        {
            Dictionary<String, Translation> lst = new Dictionary<String, Translation>();
            var langs = GetResources(@"([Rl]esources\.[Ll]ang\..*\.json)$");
#if NET5_0 || NETCOREAPP3_1
            DefaultFileLoc ??= langs.Where(x => x.Contains($"{DefaultLangInfo.Code}.json", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
#else
            DefaultFileLoc ??= langs.Where(x => x.ToLower().Contains($"{DefaultLangInfo.Code}.json".ToLower())).FirstOrDefault();
#endif
            for (int i = 0; i < langs.Count; i++)
            {
                var fileLoc = langs.ToArray()[i];
                var fileName = fileLoc.Split('.')[fileLoc.Split('.').Length - 2];
                lst.Add(fileName, LoadTranslation(new LanguageInfo(fileName), fileLoc, DefaultLangInfo, DefaultFileLoc));
            }
            return lst;
        }
#endregion


#region FromCode
        public Translation FromCode(LanguageInfo language)
        {
            return LanguageList != null && language.Code.ToLower() != "{default}" && LanguageList.Where(x => x.Key.Contains(language.Code)).ToList().Count > 0 ? LanguageList.Where(x => x.Key.Contains(language.Code)).FirstOrDefault().Value : Default;
        }
#endregion

#endregion

#region GetResources
        /// <summary>
        /// Returns a dictionary of the assembly resources (not embedded).<br/>
        /// Code from <b>Aurelien Ribon</b>
        /// </summary>
        /// <param name="filter">A regex filter for the resource paths.</param>
        public static IList<string> GetResources(string filter)
        {
            var resources = TranslationManager.ParentAssembly.GetManifestResourceNames();

            IList<string> ret = new List<string>();
            foreach (String res in resources)
            {
                if (Regex.IsMatch(res, filter))
                    ret.Add(res);
            }
            return ret;
        }
#endregion

    }
}
