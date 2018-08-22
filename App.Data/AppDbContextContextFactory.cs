using App.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace App.Data
{
    public class AppDbContextContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <inheritdoc />
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddUserSecrets("ServiceFabricSample.App.Data");

            var configuration = builder.Build();

            return new AppDbContext(new AppContextSettings(configuration),
                new Logger<AppDbContext>(new LoggerFactory()));
        }
    }
}
