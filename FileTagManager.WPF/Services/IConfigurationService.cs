using System;

namespace FileTagManager.WPF.Services
{
    public interface IConfigurationService
    {
        string Get(string key, string defaultValue);
        T Get<T>(Func<string, T> parseFunc, string key, T defaultValue);
        void Set(string value, string key);
    }
}
