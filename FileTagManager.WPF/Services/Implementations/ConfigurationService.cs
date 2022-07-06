using System;
using System.Configuration;

namespace FileTagManager.WPF.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly Configuration _config;

        public ConfigurationService()
        {
            _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string Get(string key, string defaultValue = null)
        {
            string rawConfigValue = ConfigurationManager.AppSettings[key];
            return !string.IsNullOrEmpty(rawConfigValue) ?
                rawConfigValue :
                defaultValue;
        }

        public T Get<T>(Func<string, T> parseFunc, string key, T defaultValue = default)
        {
            string rawConfigValue = ConfigurationManager.AppSettings[key];
            return !string.IsNullOrEmpty(rawConfigValue) ?
                parseFunc(rawConfigValue) :
                defaultValue;
        }

        public void Set(string value, string key)
        {
            if (ConfigurationManager.AppSettings[key] == null) return;

            _config.AppSettings.Settings[key].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
