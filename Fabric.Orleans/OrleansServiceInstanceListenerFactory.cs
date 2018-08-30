using App.Core.Interfaces.Data;
using App.Core.Models.Entities;
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
        /// <summary>
        /// This factory creates a new listener for the Oreleans
        /// tier of our application. The table storage connection 
        /// strings and other configuration information is presented 
        /// in FabricSettings. 
        /// </summary>
        /// <param name="settings">The <see cref="FabricSettings"/></param>
        /// <returns>A <see cref="ServiceInstanceListener"/> </returns>
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

            //**************************************************************
            //* Author : Jacob Atchley
            //* Date   : 08:29:2018 04:00 PM
            //* Comment: Bound to MyStatelessService/PackageRoot/Config/ServiceManifest.xml
            //**************************************************************
            var siloEndpoint = endpoints["OrleansSiloEndpoint"];
            var gatewayEndpoint = endpoints["OrleansProxyEndpoint"];
            var hostname = context.NodeContext.IPAddressOrFQDN;



            builder.UseDashboard(options =>
            {
                options.Port = endpoints["OrleansDashboard"].Port;
            });

            builder.ConfigureEndpoints(hostname, siloEndpoint.Port, gatewayEndpoint.Port);

            builder.ConfigureApplicationParts(parts =>
            {
                parts.AddApplicationPart(typeof(MyFirstGrain).Assembly).WithReferences();
                parts.AddFromApplicationBaseDirectory();
            });

            builder.ConfigureServices((ctx, coll) => ConfigureServices(coll, settings));

        }

        //**************************************************************
        //* Author : Jacob Atchley
        //* Date   : 08:29:2018 03:57 PM
        //* Comment: Adds our services to the Orleans IoC Container
        //**************************************************************
        private static void ConfigureServices(IServiceCollection services, FabricSettings settings)
        {
            services.AddSingleton<IAppContextSettings>(new AppContextSettings(settings.Db));
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<ICrudRepo<Guid, Person>, PeopleRepository>();
        }
    }
}
