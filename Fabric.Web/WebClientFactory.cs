using App.Data.Interfaces;
using Fabric.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans.Client;
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

                var builder = new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureServices(services => services.AddSingleton(new HttpClient())
                        .AddSingleton(new FabricClient())
                        .AddSingleton(x));

                if (options.FabricSettings.IsLocal)
                {
                    builder = builder.UseContentRoot(options.ContentRoot);
                }

                builder = builder
                    .ConfigureServices(c => ConfigureServices(c, options))
                    .UseStartup<Startup>()
                    .UseApplicationInsights()
                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                    .UseUrls(url);

                return builder.Build();
            }));
        }

        private static void ConfigureServices(IServiceCollection services,
            WebClientFactoryOptions options)
        {
            services.AddScoped(provider => OrleansClientFactory.Get(
                "fabric:/ServiceFabricSample/MyStatelessService", options.FabricSettings.TableStorage));

            services.AddSingleton<IAppContextSettings>(new AppContextSettings(options.FabricSettings.Db));

            services.AddSingleton(options.FabricSettings);
        }
    }
}
