
using GalaSoft.MvvmLight;
using System;
using Windows.Storage;

namespace PlantSitterShared.Common
{
    public class AppSettings : ViewModelBase
    {
        public ApplicationDataContainer LocalSettings { get; set; }

        public AppSettings()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
        }

        protected void SaveSettings(string key, object value)
        {
            LocalSettings.Values[key] = value;
        }

        protected T ReadSettings<T>(string key, T defaultValue)
        {
            if (LocalSettings.Values.ContainsKey(key))
            {
                return (T)LocalSettings.Values[key];
            }
            if (defaultValue != null)
            {
                return defaultValue;
            }
            return default(T);
        }

        private static readonly Lazy<AppSettings> lazy = new Lazy<AppSettings>(() => new AppSettings());

        public static AppSettings Instance { get { return lazy.Value; } }
    }
}
