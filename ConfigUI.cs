using SRML.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace ModConfigManager
{
    public static class ConfigUI
    {
        private static GameObject rootPanel;
        private static InputField searchField;
        private static Dropdown languageDropdown;
        private static Dropdown themeDropdown;
        private static Button saveButton;
        private static Button resetButton;
        private static GameObject configContainer;

        private static List<ConfigEntry> allEntries = new List<ConfigEntry>();

        public static void CreateUI(TabbedMenu menu)
        {
            var tab = menu.AddTab(LocalizationManager.Get("mod_title"));
            rootPanel = tab.CreateVerticalLayout();

            // Панель с языком и темой
            var headerPanel = UIUtils.CreateHorizontalGroup(rootPanel, spacing: 10);
            languageDropdown = UIUtils.CreateDropdown(headerPanel, GetLanguageList(), LocalizationManager.CurrentLanguage);
            languageDropdown.onValueChanged.AddListener(OnLanguageChanged);

            themeDropdown = UIUtils.CreateDropdown(headerPanel, new List<string> {
                LocalizationManager.Get("theme_default"),
                LocalizationManager.Get("theme_dark"),
                LocalizationManager.Get("theme_light"),
                LocalizationManager.Get("theme_purple")
            }, 0);
            themeDropdown.onValueChanged.AddListener(OnThemeChanged);

            // Строка поиска
            searchField = UIUtils.CreateInputField(rootPanel, LocalizationManager.Get("search_placeholder"));
            searchField.onValueChanged.AddListener(FilterConfigList);

            // Контейнер настроек
            configContainer = UIUtils.CreateVerticalGroup(rootPanel);

            // Кнопки управления
            var buttonPanel = UIUtils.CreateHorizontalGroup(rootPanel, spacing: 10);
            saveButton = UIUtils.CreateButton(buttonPanel, LocalizationManager.Get("save_button"), OnSaveClicked);
            resetButton = UIUtils.CreateButton(buttonPanel, LocalizationManager.Get("reset_button"), OnResetClicked);

            LoadAllConfigs();
            RenderConfigEntries();
        }

        private static List<string> GetLanguageList()
        {
            return new List<string> { "en", "ru", "ja", "zh" };
        }
private static void OnLanguageChanged(int index)
        {
            var langCode = GetLanguageList()[index];
            LocalizationManager.SetLanguage(langCode);
            UIUtils.GetTabbedMenu().RemoveTab(LocalizationManager.Get("mod_title"));
            CreateUI(UIUtils.GetTabbedMenu());
        }

        private static void OnThemeChanged(int index)
        {
            ThemeManager.ApplyTheme(index);
        }

        private static void OnSaveClicked()
        {
            ConfigStorage.SaveAll(allEntries);
        }

        private static void OnResetClicked()
        {
            ConfigStorage.ResetAll(allEntries);
            RenderConfigEntries();
        }

        private static void FilterConfigList(string input)
        {
            RenderConfigEntries(input);
        }

        private static void LoadAllConfigs()
        {
            allEntries.Clear();

            foreach (var mod in SRModLoader.Instance.Mods)
            {
                if (mod.Config == null) continue;

                foreach (var kvp in mod.Config.Data)
                {
                    allEntries.Add(new ConfigEntry(
                        mod.ID,
                        kvp.Key,
                        kvp.Value,
                        mod.Config.DefaultData.ContainsKey(kvp.Key) ? mod.Config.DefaultData[kvp.Key] : kvp.Value,
                        mod.Config.Descriptions.ContainsKey(kvp.Key) ? mod.Config.Descriptions[kvp.Key] : ""
                    ));
                }
            }
        }

        private static void RenderConfigEntries(string filter = "")
        {
            foreach (Transform child in configContainer.transform)
                Object.Destroy(child.gameObject);

            var filtered = string.IsNullOrEmpty(filter)
                ? allEntries
                : allEntries.Where(e =>
                    e.Key.ToLower().Contains(filter.ToLower()) ||
                    e.ModID.ToLower().Contains(filter.ToLower())).ToList();

            foreach (var entry in filtered)
            {
                var entryPanel = UIUtils.CreateHorizontalGroup(configContainer, spacing: 10);
                var label = UIUtils.CreateLabel(entryPanel, $"{entry.ModID}.{entry.Key}");

                if (!string.IsNullOrEmpty(entry.Description))
                    UIUtils.AddTooltip(label.gameObject, entry.Description);

                if (entry.Value is bool)
                {
                    var toggle = UIUtils.CreateToggle(entryPanel, (bool)entry.Value);
                    toggle.onValueChanged.AddListener(val => entry.Value = val);
                }
                else if (entry.Value is int)
                {
                    var input = UIUtils.CreateInputField(entryPanel, entry.Value.ToString());
                    input.contentType = InputField.ContentType.IntegerNumber;
                    input.onValueChanged.AddListener(val => {
                        if (int.TryParse(val, out int result))
                            entry.Value = result;
                    });
                }
                else if (entry.Value is float)
                {
                    var input = UIUtils.CreateInputField(entryPanel, entry.Value.ToString());
                    input.contentType = InputField.ContentType.DecimalNumber;
                    input.onValueChanged.AddListener(val => {
                        if (float.TryParse(val, out float result))
                            entry.Value = result;
                    });
                }
                else
                {
                    var input = UIUtils.CreateInputField(entryPanel, entry.Value.ToString());
                    input.onValueChanged.AddListener(val => entry.Value = val);
                }
            }
        }
    }
}
