using App.Core.Interfaces.Data;
using App.Core.Models;
using App.Data;
using App.Data.Implementations;
using App.Data.Interfaces;
using Fabric.Orleans.Implementations;
using Grains.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Hosting.ServiceFabric;
using System;
using System.Fabric;
using System.IO;

namespace Fabric.Orleans
{
    public static class OrleansServiceInstanceListenerFactory
    {
        public static ServiceInstanceListener Get()
        {
            return OrleansServiceListener.CreateStateless(Configure);
        }

        private static void Configure(StatelessServiceContext context, ISiloHostBuilder builder)
        {
            builder.ConfigureServices(ConfigureServices);

            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = context.ServiceName.ToString();
                options.ClusterId = "development";
            });

            builder.UseAzureStorageClustering(options => options.ConnectionString = "UseDevelopmentStorage=true");

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

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            var daRoot = (new DirectoryInfo(Directory.GetCurrentDirectory()).Parent?.FullName ?? string.Empty) + "\\Fabric.Web";

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"{daRoot}\\appsettings.secrets.json", true);

            var configuration = builder.Build();

            services.AddSingleton<IAppContextSettings>(
                new AppContextSettingsOrleans(configuration.GetConnectionString("Db")));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();
        }
    }
}
