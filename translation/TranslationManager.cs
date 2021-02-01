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

    public class TranslationManager : LanguageInfo
    {

        #region Properties
        internal static Assembly ParentAssembly { get; set; }

        private IList<string> LanguageFileNames { get; }
        private Dictionary<string, Translation> LanguageList { get; }
        private string Extension { get; }

        protected LanguageInfo DefaultLangInfo { get; set; } = new LanguageInfo("en");
        protected LanguageInfo LangInfo = null;

        public Translation Current { get; internal set; }
        public Translation Default { get; internal set; }
        public string DefaultFileLoc { get; private set; }
        #endregion

        /// <summary>
        /// Manage translations from assembly<i> or in the future, you'll be able to get translations from web and file as well</i>.
        /// </summary>
        /// <param name="parentAssembly">This is important to set it correctly, because this is used to locate language files. Language file directory is <b>Resources\Lang\lang_CODE.<paramref name="extension"/></b><br/>If you disable preload, the <see cref="Default"/> and <see cref="Current"/> properties can't be used.</param>
        /// <param name="langCode">Language code (system based language code, for example <b>en_US</b> or <b>en</b>.</param>
        /// <param name="preloadTranslations">If set to <b>true</b>, this manager will automatically load the language list. It can be disabled by set to <b>false</b>, which is useful if you'd like to handle language on your own.</param>
        /// <param name="extension">Extension of the language file. This can be used to alter the default extension, which is <b>json</b></param>
        public TranslationManager(Assembly parentAssembly, string langCode = "{default}", bool preloadTranslations = true, string extension = "json")
        {
            extension ??= "json";
            Extension = extension;
            ParentAssembly = parentAssembly ?? Assembly.GetCallingAssembly();
            if (preloadTranslations)
            {
                langCode = langCode == "{default}" || langCode.ToLower() == DefaultLangInfo.Code.ToLower() ? null : langCode;
                CultureInfo lang = null;
                LanguageFileNames = GetResources(@$"([Rr]esources\.[Ll]ang\..*\.{Extension})$");
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
        }

        #region Loading stuff

        #region GetTranslation
        protected Translation LoadTranslation(LanguageInfo lang, string assemblyLangLoc, LanguageInfo def = null, string assemblyDefLangLoc = null, bool isDefault = false)
        {
            ///DONE: Redo GetLanguage
            Translation res = null;
            var fileLangLoc = Path.Combine(Globals.WorkingDir, "Langs", $"{lang.Code}.{Extension}");
            var fileDefLangLoc = Path.Combine(Globals.WorkingDir, "Langs", $"en.{Extension}");
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
                    Logger.Error("Cannot load any languages. If you're the application developer, please refer documentation to see more info about loading languages using our system.", true);
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
                if (!isDefault)
                {
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
                if (!isDefault)
                {
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
            var langs = GetResources(@$"([Rl]esources\.[Ll]ang\..*\.{Extension})$");
#if NET5_0 || NETCOREAPP3_1
            DefaultFileLoc ??= langs.Where(x => x.Contains($"{DefaultLangInfo.Code}.{Extension}", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
#else
            DefaultFileLoc ??= langs.Where(x => x.ToLower().Contains($"{DefaultLangInfo.Code}.{Extension}".ToLower())).FirstOrDefault();
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
