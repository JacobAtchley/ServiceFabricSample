using App.Data.Interfaces;
using Fabric.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans.Client;
using System.Fabric;
using System.Fabric.Description;
using System.Net.Http;

namespace Fabric.Web
{
    public static class WebClientFactory
    {
        public static ServiceInstanceListener Get(WebClientFactoryOptions options)
        {
            return new ServiceInstanceListener(x => new KestrelCommunicationListener(x, options.EndpointName, (url, listener) =>
            {

                var activation = x.CodePackageActivationContext;
                var settings = activation.GetConfigurationPackageObject("Config").Settings;
                var myConfig = settings.Sections["MyConfigSection"];

                options.LogAction(x, $"Starting Kestrel. Earl: {url}. Content Directory: {options.ContentDirectory}");

                return new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureServices(services => services.AddSingleton(new HttpClient())
                        .AddSingleton(new FabricClient())
                        .AddSingleton(x))
                    .UseContentRoot(options.ContentDirectory)
                    .ConfigureServices(c => ConfigureServices(c, myConfig))
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                    .UseUrls(url)
                    .Build();
            }));
        }

        private static void ConfigureServices(IServiceCollection services,
            ConfigurationSection configSection)
        {
            var tableStorage = configSection.Parameters["TableStorageConnectionString"].Value;
            var db = configSection.Parameters["DbConnectionString"].Value;

            services.AddScoped(provider => OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService",
                tableStorage));

            services.AddSingleton<IAppContextSettings>(new AppContextSettings(db));
        }
    }
}
