using Microsoft.Extensions.Configuration;
using System;

namespace App.Data.Interfaces
{
    public interface IAppContextSettings
    {
        string ConnectionString { get; }
    }

    public class AppContextSettings : IAppContextSettings
    {
        public AppContextSettings(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("Db");

            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new ArgumentException(
                    $"Could not find connection string for {nameof(AppContextSettings)}");
            }
        }

        public AppContextSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <inheritdoc />
        public string ConnectionString { get; }
    }
}
