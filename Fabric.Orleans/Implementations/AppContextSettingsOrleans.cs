using App.Data.Interfaces;

namespace Fabric.Orleans.Implementations
{
    public class AppContextSettingsOrleans : IAppContextSettings
    {
        public AppContextSettingsOrleans(string conn)
        {
            ConnectionString = conn;
        }
        /// <inheritdoc />
        public string ConnectionString { get; }
    }
}
