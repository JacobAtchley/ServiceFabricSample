using Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace OrleansStateless.Client
{
    public static class OrleansClientFactory
    {
        public static IClusterClient Get()
        {
            var serviceName = new Uri("fabric:/ServiceFabricSample/OrleansStateless");

            var builder = new ClientBuilder();

            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = serviceName.ToString();
                options.ClusterId = "development";
            });

            builder.UseAzureStorageClustering(options => options.ConnectionString = "UseDevelopmentStorage=true");

            builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IMyFirstGrain).Assembly));

            builder.ConfigureLogging(logging => logging.AddDebug());

            var client = builder.Build();

            return client;
        }
    }
}
