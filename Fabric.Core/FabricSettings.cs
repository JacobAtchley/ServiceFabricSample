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

                Db = MyConfigurationSection.Parameters
                    .TryGetValue("DbConnectionString", out var db)
                    ? db.Value
                    : null;

                TableStorage = MyConfigurationSection.Parameters
                    .TryGetValue("TableStorageConnectionString", out var table)
                    ? table.Value
                    : null;

                IsLocal = MyConfigurationSection.Parameters
                    .TryGetValue("IsLocal", out var isLocal)
                    && bool.TryParse(isLocal.Value, out var isLocalBoolean)
                    && isLocalBoolean;

                if (string.IsNullOrWhiteSpace(Db))
                {
                    throw new Exception("Could not get db connection string from configuration package");
                }

                if (string.IsNullOrWhiteSpace(TableStorage))
                {
                    throw new Exception("Could not get table storage connection string from configuration package");
                }
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
    }
}
