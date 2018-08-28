using App.Core.Models;
using Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace Orleans.Client
{
    public static class OrleansClientFactory
    {
        public static IClusterClient Get(OrleansClientConnectionOptions options)
        {
            var serviceName = new Uri(options.FabricUrl);

            var builder = new ClientBuilder();

            builder.Configure<ClusterOptions>(opt =>
            {
                opt.ServiceId = serviceName.ToString();
                opt.ClusterId = options.ClusterId;
            });

            builder.UseAzureStorageClustering(opt =>
                opt.ConnectionString = options.TableStorageConnectionString);

            builder.ConfigureApplicationParts(parts =>
                parts.AddApplicationPart(typeof(IMyFirstGrain).Assembly));

            builder.ConfigureLogging(logging => logging.AddDebug());

            var client = builder.Build();

            return client;
        }
    }
}
