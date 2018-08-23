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

        /// <inheritdoc />
        public string ConnectionString { get; }
    }
}
