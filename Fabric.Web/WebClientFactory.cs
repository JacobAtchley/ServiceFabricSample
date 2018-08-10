using Fabric.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System.Fabric;
using System.Net.Http;

namespace Fabric.Web
{
    public static class WebClientFactory
    {
        public static ServiceInstanceListener Get(WebClientFactoryOptions options)
        {
            return new ServiceInstanceListener(x => new KestrelCommunicationListener(x, options.EndpointName, (url, listener) =>
            {

                options.LogAction(x, $"Starting Kestrel. Earl: {url}. Content Directory: {options.ContentDirectory}");

                return new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureServices(services => services.AddSingleton(new HttpClient())
                        .AddSingleton(new FabricClient())
                        .AddSingleton(x))
                    .UseContentRoot(options.ContentDirectory)
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                    .UseUrls(url)
                    .Build();
            }));
        }
    }
}
