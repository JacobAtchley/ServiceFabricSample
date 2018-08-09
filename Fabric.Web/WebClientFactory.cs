using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using System;
using System.Fabric;
using System.IO;
using System.Net.Http;

namespace Fabric.Web
{
    public static class WebClientFactory
    {
        public static ServiceInstanceListener Get(string endpointName, Action<StatelessServiceContext, string> logAction)
        {
            return new ServiceInstanceListener(x => CreateCommunicationListener(x, logAction, endpointName));
        }

        private static ICommunicationListener CreateCommunicationListener(StatelessServiceContext serviceContext,
            Action<StatelessServiceContext, string> logAction, string endpointName)
        {
            return new KestrelCommunicationListener(serviceContext, endpointName, (url, listener) =>
            {
                logAction(serviceContext, $"Starting Kestrel on {url}");

                return new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureServices(services => services.AddSingleton(new HttpClient())
                        .AddSingleton(new FabricClient())
                        .AddSingleton(serviceContext))
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                    .UseUrls(url)
                    .Build();
            });
        }
    }
}
