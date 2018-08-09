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
        public static IClusterClient Get(string fabricUrl, string azureStorageConnectionString)
        {
            var serviceName = new Uri(fabricUrl);

            var builder = new ClientBuilder();

            builder.Configure<ClusterOptions>(options =>
            {
                options.ServiceId = serviceName.ToString();
                options.ClusterId = "development";
            });

            builder.UseAzureStorageClustering(options => options.ConnectionString = azureStorageConnectionString);

            builder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IMyFirstGrain).Assembly));

            builder.ConfigureLogging(logging => logging.AddDebug());

            var client = builder.Build();

            return client;
        }
    }
}
