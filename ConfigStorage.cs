using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace ModConfigManager
{
    public static class ConfigStorage
    {
        public static void SaveAll(List<ConfigEntry> entries)
        {
            var mods = entries.GroupBy(e => e.ModID);
            foreach (var modGroup in mods)
            {
                var mod = SRModLoader.Instance.Mods.FirstOrDefault(m => m.ID == modGroup.Key);
                if (mod == null || mod.Config == null)
                    continue;

                foreach (var entry in modGroup)
                {
                    mod.Config.Data[entry.Key] = entry.Value;
                }

                SaveToFile(mod);
            }
        }

        public static void ResetAll(List<ConfigEntry> entries)
        {
            foreach (var entry in entries)
            {
                entry.Value = entry.DefaultValue;
            }
        }

        private static void SaveToFile(SRMod mod)
        {
            string path = Path.Combine(SRModLoader.ModsFolder, mod.ID, "config.json");
            var json = JsonConvert.SerializeObject(mod.Config.Data, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
