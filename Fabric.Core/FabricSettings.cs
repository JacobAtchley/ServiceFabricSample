using System;
using System.Fabric;
using System.Fabric.Description;

namespace Fabric.Core
{
    public class FabricSettings
    {
        public FabricSettings(ConfigurationPackage package)
        {
            Package = package;
            ConfigurationSettings = Package.Settings;

            if (ConfigurationSettings.Sections.TryGetValue("MyConfigSection", out var configSection))
            {
                MyConfigurationSection = configSection;

                Db = GetSettingString("DbConnectionString");

                if (string.IsNullOrWhiteSpace(Db))
                {
                    throw new Exception("Could not get db connection string from configuration package");
                }

                TableStorage = GetSettingString("TableStorageConnectionString");

                if (string.IsNullOrWhiteSpace(TableStorage))
                {
                    throw new Exception("Could not get table storage connection string from configuration package");
                }

                SignalRConnectionString = GetSettingString("SignalRConnectionString");

                if (string.IsNullOrWhiteSpace(SignalRConnectionString))
                {
                    throw new Exception("Could not get signal r connection string from configuration package");
                }

                IsLocal = bool.TryParse(GetSettingString("IsLocal"), out var isLocalBoolean) && isLocalBoolean;
            }
            else
            {
                throw new Exception("Could not get my configuration section");
            }
        }

        public string Db { get; }

        public string TableStorage { get; }

        public bool IsLocal { get; }

        public ConfigurationPackage Package { get; }

        public ConfigurationSettings ConfigurationSettings { get; }

        public ConfigurationSection MyConfigurationSection { get; }

        public string SignalRConnectionString { get; }

        private string GetSettingString(string name)
        {
            return MyConfigurationSection.Parameters
                .TryGetValue(name, out var settingValue)
                ? settingValue.Value
                : null;
        }
    }
}
