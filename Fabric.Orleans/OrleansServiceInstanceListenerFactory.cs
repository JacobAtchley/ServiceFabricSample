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
        public static ServiceInstanceListener Get(ConfigurationSettings settings)
        {
            return OrleansServiceListener.CreateStateless((context, builder) => Configure(context, builder, settings));
        }

        private static void Configure(ServiceContext context, ISiloHostBuilder builder, ConfigurationSettings settings)
        {
            builder.ConfigureServices((ctx, coll) => ConfigureServices(ctx, coll, settings));

            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = context.ServiceName.ToString();
                options.ClusterId = "development";
            });

            var tableStorage = settings.Sections["MyConfigSection"].Parameters["TableStorageConnectionString"].Value;

            builder.UseAzureStorageClustering(options => options.ConnectionString = tableStorage);

            builder.ConfigureLogging(logging => logging.AddDebug());

            var activation = context.CodePackageActivationContext;
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
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services, ConfigurationSettings settings)
        {
            var db = settings.Sections["MyConfigSection"].Parameters["DbConnectionString"].Value;

            services.AddSingleton<IAppContextSettings>(new AppContextSettings(db));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();
        }
    }
}
