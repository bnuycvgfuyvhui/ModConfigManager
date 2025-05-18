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
