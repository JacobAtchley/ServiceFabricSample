using Microsoft.Extensions.Configuration;

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
        }

        /// <inheritdoc />
        public string ConnectionString { get; }
    }
}
