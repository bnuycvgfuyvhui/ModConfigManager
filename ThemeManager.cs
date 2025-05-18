using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ModConfigManager
{
    public static class ThemeManager
    {
        private static readonly Dictionary<string, Color> LightTheme = new()
        {
            { "Background", new Color(1f, 1f, 1f, 1f) },
            { "Text",       new Color(0f, 0f, 0f, 1f) },
            { "Button",     new Color(0.9f, 0.9f, 0.9f, 1f) }
        };

        private static readonly Dictionary<string, Color> DarkTheme = new()
        {
            { "Background", new Color(0.12f, 0.12f, 0.12f, 1f) },
            { "Text",       new Color(1f, 1f, 1f, 1f) },
            { "Button",     new Color(0.25f, 0.25f, 0.25f, 1f) }
        };

        private static readonly Dictionary<string, Color> PurpleTheme = new()
        {
            { "Background", new Color(0.25f, 0.15f, 0.35f, 1f) },
            { "Text",       new Color(1f, 0.9f, 1f, 1f) },
            { "Button",     new Color(0.5f, 0.25f, 0.75f, 1f) }
        };

        public static int CurrentThemeIndex { get; private set; } = 0;

        public static void ApplyTheme(int index)
        {
            CurrentThemeIndex = index;
            var theme = index switch
            {
                1 => DarkTheme,
                2 => LightTheme,
                3 => PurpleTheme,
                _ => LightTheme
            };

            foreach (var text in Object.FindObjectsOfType<Text>())
                text.color = theme["Text"];

            foreach (var img in Object.FindObjectsOfType<Image>())
            {
                if (img.gameObject.name.Contains("Panel"))
                    img.color = theme["Background"];
                else if (img.gameObject.name.Contains("Button"))
                    img.color = theme["Button"];
            }
        }
    }
}
