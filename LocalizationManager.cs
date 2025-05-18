using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ModConfigManager
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> localizedText;
        private static string languageFilePath => Path.Combine(SRModLoader.ModsFolder, "ModConfigManager", "Language.json");
        public static string CurrentLanguage { get; private set; } = "en";

        public static void LoadLanguage()
        {
            if (File.Exists(languageFilePath))
            {
                var lang = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(languageFilePath));
                if (lang != null && lang.ContainsKey("CurrentLanguage"))
                    CurrentLanguage = lang["CurrentLanguage"];
            }

            string path = Path.Combine(SRModLoader.ModsFolder, "ModConfigManager", "Languages", $"{CurrentLanguage}.json");
            if (File.Exists(path))
                localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
            else
                localizedText = new Dictionary<string, string>();
        }

        public static string Get(string key)
        {
            return localizedText.ContainsKey(key) ? localizedText[key] : key;
        }

        public static void SetLanguage(string langCode)
        {
            CurrentLanguage = langCode;
            File.WriteAllText(languageFilePath, JsonConvert.SerializeObject(new Dictionary<string, string>
            {
                { "CurrentLanguage", langCode }
            }));
            LoadLanguage();
        }
    }
}
