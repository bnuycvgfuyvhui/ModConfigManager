namespace ModConfigManager
{
    public class ConfigEntry
    {
        public string ModID { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public object DefaultValue { get; set; }
        public string Description { get; set; }

        public ConfigEntry(string modID, string key, object value, object defaultValue, string description)
        {
            ModID = modID;
            Key = key;
            Value = value;
            DefaultValue = defaultValue;
            Description = description;
        }
    }
}
