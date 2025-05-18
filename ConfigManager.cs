using SRML;
using SRML.Console;
using SRML.ModLoading;
using SRML.UI;
using UnityEngine;

namespace ModConfigManager
{
    public class ConfigManager : ModEntryPoint
    {
        public override void PreLoad()
        {
            SRConsole.Write("ModConfigManager PreLoad()");
        }

        public override void Load()
        {
            SceneContext.Instance?.GetComponentInChildren<TabbedMenu>()?.AddTab(LocalizationManager.Get("mod_title"));
            LocalizationManager.LoadLanguage();
            UIUtils.OnMainMenuOpened += () =>
            {
                ConfigUI.CreateUI(UIUtils.GetTabbedMenu());
            };
        }

        public void ReloadUI()
        {
            UIUtils.GetTabbedMenu()?.RemoveTab(LocalizationManager.Get("mod_title"));
            ConfigUI.CreateUI(UIUtils.GetTabbedMenu());
        }
    }
}
