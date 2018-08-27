using App.Core.Interfaces.Data;
using App.Core.Models;
using App.Data;
using App.Data.Implementations;
using App.Data.Interfaces;
using Grains.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Hosting.ServiceFabric;
using System;
using System.Fabric;
using System.Fabric.Description;

namespace Fabric.Orleans
{
    public static class OrleansServiceInstanceListenerFactory
    {
        public static ServiceInstanceListener Get()
        {
            return OrleansServiceListener.CreateStateless(Configure);
        }

        private static void Configure(ServiceContext context, ISiloHostBuilder builder)
        {
            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = context.ServiceName.ToString();
                options.ClusterId = "development";
            });


            builder.ConfigureLogging(logging => logging.AddDebug());

            var activation = context.CodePackageActivationContext;
            var settings = activation.GetConfigurationPackageObject("Config").Settings;
            var myConfig = settings.Sections["MyConfigSection"];

            builder.UseAzureStorageClustering(options =>
                options.ConnectionString = myConfig.Parameters["TableStorageConnectionString"].Value);

            var endpoints = activation.GetEndpoints();

            var siloEndpoint = endpoints["OrleansSiloEndpoint"];
            var gatewayEndpoint = endpoints["OrleansProxyEndpoint"];
            var hostname = context.NodeContext.IPAddressOrFQDN;

            builder.ConfigureEndpoints(hostname, siloEndpoint.Port, gatewayEndpoint.Port);

            builder.ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(MyFirstGrain).Assembly).WithReferences();
                parts.AddFromApplicationBaseDirectory();
            });

            builder.ConfigureServices((ctx, coll) => ConfigureServices(coll, myConfig));

        }

        private static void ConfigureServices(IServiceCollection services, ConfigurationSection settings)
        {
            var db = settings.Parameters["DbConnectionString"].Value;

            services.AddSingleton<IAppContextSettings>(new AppContextSettings(db));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();
        }
    }
}
