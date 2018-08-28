using App.Core.Interfaces.Data;
using App.Core.Models;
using App.Data;
using App.Data.Implementations;
using App.Data.Interfaces;
using Fabric.Core;
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

namespace Fabric.Orleans
{
    public static class OrleansServiceInstanceListenerFactory
    {
        public static ServiceInstanceListener Get(FabricSettings settings)
        {
            return OrleansServiceListener.CreateStateless(
                (ctx, builder) => Configure(ctx, builder, settings));
        }

        private static void Configure(ServiceContext context, ISiloHostBuilder builder, FabricSettings settings)
        {
            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = context.ServiceName.ToString();
                options.ClusterId = settings.IsLocal ? "development" : "production";
            });


            builder.ConfigureLogging(logging => logging.AddDebug());


            builder.UseAzureStorageClustering(options =>
                options.ConnectionString = settings.TableStorage);

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

            builder.ConfigureServices((ctx, coll) => ConfigureServices(coll, settings));

        }

        private static void ConfigureServices(IServiceCollection services, FabricSettings settings)
        {
            services.AddSingleton<IAppContextSettings>(new AppContextSettings(settings.Db));
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();
        }
    }
}
